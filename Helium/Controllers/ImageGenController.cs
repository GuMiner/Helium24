using Helium.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web;

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
        private const string ImageCreatorServer = "http://73.53.3.16:7589/api/ImageCreator";

        private readonly ImageDbContext db;
        private readonly IHttpClientFactory httpClientFactory;

        public ImageGenController(ImageDbContext db, IHttpClientFactory httpClientFactory)
        {
            this.db = db;
            this.httpClientFactory = httpClientFactory;
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
                        .Select(job => $"{job.Id}: {job.Prompt}").ToList();

                return this.Ok(new
                {
                    jobs = jobs,
                    userName = user.Name,
                    error = "",
                });
            });
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

                // Read the image from local storage if possible
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
                        imageData = image,
                    });
                }

                // The image might be generated, but not uploaded yet. Grab it.
                HttpRequestMessage request = new HttpRequestMessage(
                    HttpMethod.Get,
                    $"{ImageCreatorServer}/Get?jobId={job.Id}");

                HttpClient httpClient = httpClientFactory.CreateClient();
                HttpResponseMessage response = httpClient.Send(request);
                if (!response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return this.Ok(new
                    {
                        error = $"Could not read job info from the remote server: {responseString}",
                    });
                }

                byte[] imageData = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                if (imageData.Length == 0)
                {
                    return this.Ok(new
                    {
                        error = "",
                        status = "Image not ready yet",
                        imageData = "",
                    });
                }

                // Have image, save it to this job.
                string imageId = Guid.NewGuid().ToString();
                job.ImageIds = imageId; // TODO multiple job support
                db.Jobs.Update(job);
                db.Images.Add(new GeneratedImage()
                {
                    Id = imageId,
                    ImageData = imageData,
                });

                db.SaveChanges();

                return this.Ok(new
                {
                    error = "",
                    status = "",
                    imageData = imageData,
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
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{ImageCreatorServer}/Queue?prompt={HttpUtility.UrlEncode(prompt)}");

            HttpClient httpClient = httpClientFactory.CreateClient();
            HttpResponseMessage response = httpClient.Send(request);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            string jobID = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

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