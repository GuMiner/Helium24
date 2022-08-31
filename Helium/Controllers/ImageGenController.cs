using Helium.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [AcceptVerbs("GET")]
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

        [AcceptVerbs("POST")]
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

                string jobId = this.SendJobToBackend(user.Id, prompt);

                return this.Ok(new
                {
                    userName = user.Name,
                    jobId = jobId,
                    error = "",
                });
            });
        }

        private string SendJobToBackend(long userId, string prompt)
        {
            // TODO implement backend
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

            this.db.SaveChanges();

            return jobID;
        }

        private bool IsValidUser(string accessKey, out User user)
        {
            user = db.Users.FirstOrDefault(user => user.AccessKey.Equals(accessKey));
            return user != null;
        }
    }
}