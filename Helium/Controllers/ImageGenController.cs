using Helium.Data;
using Microsoft.AspNetCore.Mvc;

namespace Helium.Controllers
{
    /// <summary>
    /// Module for retrieving crossword results
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class ImageGenController : ControllerBase
    {
        private readonly ImageDbContext db;

        public ImageGenController(ImageDbContext db)
        {
            this.db = db;
        }

        private IActionResult Login(string? accessKey, Func<User, IActionResult> postLoginAction)
        {
            accessKey ??= string.Empty;
            accessKey = accessKey.ToUpperInvariant();

            if (this.IsValidUser(accessKey, out User user))
            {
                return postLoginAction(user);
            }

            return this.Ok(new
            {
                error = "Invalid user!"
            });
        }

        [HttpGet]
        public IActionResult PendingJobsCount([FromQuery] string? accessKey)
        {
            return this.Login(accessKey, (user) =>
            {
                return this.Ok(new
                {
                    count = this.db.PendingJobs.LongCount(),
                    error = ""
                });
            });
        }

        [HttpGet]
        public IActionResult Jobs([FromQuery] string? accessKey)
        {
            return this.Login(accessKey, (user) =>
            {
                List<Job> jobs = this.db.UserJobs.Where(userJob => userJob.UserId.Equals(user.Id))
                    .Join(this.db.Jobs,
                        a => a.JobId,
                        b => b.Id,
                        (a, b) => b).ToList();

                // Queue up extra image generation jobs only if there are no pending jobs whatsoever
                // One-time data population only
                // if (this.db.PendingJobs.Count() < 22)
                // {
                //     foreach (Job job in this.db.Jobs)
                //     {
                //         int imageCount = job.ImageIds.Split(',').Length;
                //         if (imageCount != 4)
                //         {
                //             for (int i = imageCount; i < 4; i++)
                //             {
                //                 this.db.PendingJobs.Add(new PendingJob()
                //                 {
                //                     JobId = $"{job.Id}:{i}",
                //                     IsProcessing = 0
                //                 });
                //             }
                //         }
                //     }
                // 
                //     this.db.SaveChanges();
                // }

                return this.Ok(new
                {
                    jobs = jobs.Select(job => $"{job.Prompt}:{job.Id}").ToList(),
                    userName = user.Name,
                    error = "",
                });
            });
        }

        [HttpGet]
        public IActionResult GetNextJob()
        {
            if (this.db.PendingJobs.Any())
            {
                PendingJob firstPendingJob = this.db.PendingJobs.First();
                string jobId = firstPendingJob.JobId.Split(":").First();
                string prompt = this.db.Jobs.First(job => job.Id == jobId).Prompt;

                return this.Ok(new
                {
                    jobID = firstPendingJob.JobId,
                    prompt = prompt
                });
            }

            return this.Ok(new
            {
                jobID = "",
                prompt = ""
            });
        }

        [HttpPost]
        public IActionResult CompleteJob([FromQuery]string jobId)
        {
            db.PendingJobs.Remove(db.PendingJobs.First(job => job.JobId == jobId));

            jobId = jobId.Split(":").First();

            // TODO error checking
            // Have image, save it to this job.
            Job job = this.db.Jobs.First(job => job.Id == jobId);

            string imageId = Guid.NewGuid().ToString();
            if (string.IsNullOrWhiteSpace(job.ImageIds))
            {
                job.ImageIds = imageId;
            }
            else
            {
                job.ImageIds = $"{job.ImageIds},{imageId}";
            }

            db.Jobs.Update(job);

            using (MemoryStream reader = new MemoryStream())
            {
                Request.Body.CopyToAsync(reader).GetAwaiter().GetResult();

                db.Images.Add(new GeneratedImage()
                {
                    Id = imageId,
                    ImageData = reader.ToArray()
                });
            }

            db.SaveChanges();

            return this.Ok();
        }

        [HttpGet]
        public IActionResult JobResults([FromQuery] string? accessKey, [FromQuery]string jobId, [FromQuery]int imageIdx)
        {
            return this.Login(accessKey, (user) =>
            {
                Job? job = this.db.Jobs.FirstOrDefault(job => job.Id == jobId);
                if (job == null)
                {
                    return this.Ok(new
                    {
                        error = $"Could not find job {jobId}"
                    });
                }

                // Read the image from local storage if loaded
                if (!string.IsNullOrWhiteSpace(job.ImageIds))
                {
                    string[] imageIds = job.ImageIds.Split(',');
                    if (imageIdx >= imageIds.Length)
                    {
                        return this.Ok(new
                        {
                            error = $"Could not find image ID {job.ImageIds}, IDX {imageIdx}"
                        });
                    }

                    // TODO support comma-separated job IDs.
                    byte[]? image = this.db.Images.FirstOrDefault(img => img.Id == imageIds[imageIdx])?.ImageData;
                    if (image == null)
                    {
                        return this.Ok(new
                        {
                            error = $"DB ERROR: Could not find image ID {job.ImageIds} for job {job.Id}"
                        });
                    }

                    return this.Ok(new
                    {
                        error = "",
                        status = "",
                        imageData = $"data:image/jpeg;base64,{Convert.ToBase64String(image)}",
                    });
                }

                // Image isn't ready yet, unfortunatly
                return this.Ok(new
                {
                    error = "",
                    status = "Image not ready yet",
                    imageData = "",
                });
            });
        }

        [HttpPost]
        public IActionResult QueueJob(
            [FromQuery] string? accessKey,
            [FromQuery] string? prompt) // TODO settings and what not
        {
            return this.Login(accessKey, (user) =>
            {
                prompt ??= string.Empty;
                prompt = prompt.Replace("\r", string.Empty).Replace("\n", string.Empty).Trim();

                if (string.IsNullOrEmpty(prompt))
                {
                    return this.Ok(new
                    {
                        error = "The prompt cannot be empty!"
                    });
                }

                string? jobId = this.SendJobToBackend(user.Id, prompt);
                if (jobId == null)
                {
                    return this.Ok(new
                    {
                        error = "Could not successfully send the prompt to the GPU backend. Please try again later"
                    });
                }

                return this.Ok(new
                {
                    userName = user.Name,
                    jobId = jobId,
                    error = "",
                });
            });
        }

        private string? SendJobToBackend(long userId, string prompt)
        {
            string jobID = Guid.NewGuid().ToString();

            this.db.Jobs.Add(new Job()
            {
                Id = jobID,
                Prompt = prompt,
                ImageIds = string.Empty,
                Settings = string.Empty, // TODO implement
                IsPublic = 0,
            });

            this.db.UserJobs.Add(new UserJob()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                JobId = jobID,
                CreationDate = DateTime.UtcNow.ToString(),
            });

            // Generate four images per job. TODO customizable
            for (int i = 0; i < 4; i++)
            {
                this.db.PendingJobs.Add(new PendingJob()
                {
                    JobId = $"{jobID}:{i}",
                    IsProcessing = 0
                });
            }

            this.db.SaveChanges();

            return jobID;
        }

        private bool IsValidUser(string accessKey, out User? user)
        {
            user = db.Users.FirstOrDefault(user => user.AccessKey.Equals(accessKey));
            return user != null;
        }
    }
}