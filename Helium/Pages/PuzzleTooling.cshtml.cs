using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages
{
    public class PuzzleToolingModel : PageModel
    {
        private readonly ILogger<PuzzleToolingModel> _logger;

        public PuzzleToolingModel(ILogger<PuzzleToolingModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}