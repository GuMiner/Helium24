using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;

namespace HeliumBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ImageCreatorController : ControllerBase
    {
        private const string QueueDirectory = @"D:\imageGen";

        private readonly ILogger<ImageCreatorController> logger;

        public ImageCreatorController(ILogger<ImageCreatorController> logger)
        {
            this.logger = logger;
        }

        [HttpPost]
        public string Queue([FromQuery]string prompt)
        {
            string jobId = Guid.NewGuid().ToString();

            string directoryPath = Path.Combine(QueueDirectory, jobId);
            Directory.CreateDirectory(directoryPath);
            System.IO.File.WriteAllText(Path.Combine(directoryPath, "prompt.txt"), prompt);
            logger.LogInformation($"Queued job at {directoryPath}");

            return jobId;
        }

        [HttpGet]
        public IActionResult Get([FromQuery]string jobId) // TODO support retrieving more than one result
        {
            byte[] imageBytes = new byte[0];
            logger.LogInformation($"Retrieving job {jobId}");
            string imageFilePath = Path.Combine(QueueDirectory, jobId, "0.jpg");
            if (System.IO.File.Exists(imageFilePath))
            {
                imageBytes = System.IO.File.ReadAllBytes(imageFilePath);
            }
            
            return new FileContentResult(imageBytes.Reverse().ToArray(), "image/jpeg");
        }
    }
}