using Microsoft.AspNetCore.Mvc;

namespace HeliumBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ImageCreatorController : ControllerBase
    {
        private const string QueueDirectory = @"D:\imageGen";

        private readonly ILogger<ImageCreatorController> _logger;

        public ImageCreatorController(ILogger<ImageCreatorController> logger,)
        {
            _logger = logger;
        }

        [HttpPost]
        public string Queue([FromQuery]string prompt)
        {
            string jobId = Guid.NewGuid().ToString();

            string directoryPath = Path.Combine(QueueDirectory, jobId);
            Directory.CreateDirectory(directoryPath);
            System.IO.File.WriteAllText(Path.Combine(directoryPath, "prompt.txt"), prompt);

            return jobId;
        }

        [HttpGet]
        public byte[] Get([FromQuery]string jobId) // TODO support retrieving more than one result
        {
            string imageFilePath = Path.Combine(QueueDirectory, jobId, "0.png");
            if (System.IO.File.Exists(imageFilePath))
            {
                return System.IO.File.ReadAllBytes(imageFilePath);
            }

            return new byte[0];
        }
    }
}