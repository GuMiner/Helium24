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
                IEnumerable<string> jobs = 
                    this.db.UserJobs.Where(userJob => userJob.UserId.Equals(user.Id))
                        .Join(this.db.Jobs,
                            a => a.JobId,
                            b => b.Id,
                            (a, b) => b)
                        .Select(job => $"{job.Prompt}:{job.Id}").ToList();

                return this.Ok(new
                {
                    jobs = jobs,
                    userName = user.Name,
                    error = "",
                });
            });
        }

        [HttpGet]
        public IActionResult GetNextJob()
        {
            if (this.db.PendingJobs.Any()) // TODO thread-safe
            {
                PendingJob firstPendingJob = this.db.PendingJobs.First();
                string prompt = this.db.Jobs.First(job => job.Id == firstPendingJob.JobId).Prompt;

                firstPendingJob.IsProcessing = 1;
                this.db.SaveChanges();

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
            // TODO error checking
            // Have image, save it to this job.
            Job job = this.db.Jobs.First(job => job.Id == jobId);

            string imageId = Guid.NewGuid().ToString();
            job.ImageIds = imageId; // TODO multiple job support
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

            db.PendingJobs.Remove(db.PendingJobs.First(job => job.JobId == jobId));

            db.SaveChanges();

            return this.Ok();
        }

        [HttpGet]
        public IActionResult JobResults([FromQuery] string? accessKey, [FromQuery]string jobId)
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
                    // TODO support comma-separated job IDs.
                    byte[]? image = this.db.Images.FirstOrDefault(img => img.Id == job.ImageIds)?.ImageData;
                    if (image == null)
                    {
                        return this.Ok(new
                        {
                            error = $"Could not find image ID {job.ImageIds}"
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

            this.db.PendingJobs.Add(new PendingJob()
            {
                JobId = jobID,
                IsProcessing = 0
            });

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