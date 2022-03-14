using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages
{
    public class WeddingModel : PageModel
    {
        private readonly ILogger<WeddingModel> _logger;

        public WeddingModel(ILogger<WeddingModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}