using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages
{
    public class ProjectsModel : PageModel
    {
        private readonly ILogger<ProjectsModel> _logger;

        public ProjectsModel(ILogger<ProjectsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}