using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages
{
    public class MediaModel : PageModel
    {
        private readonly ILogger<MediaModel> _logger;
        private readonly IWebHostEnvironment webHostEnvironment;

        public string WebRootPath { get; }

        public MediaModel(ILogger<MediaModel> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            this.webHostEnvironment = webHostEnvironment;
            this.WebRootPath = this.webHostEnvironment.WebRootPath;
        }

        public void OnGet()
        {

        }
    }
}