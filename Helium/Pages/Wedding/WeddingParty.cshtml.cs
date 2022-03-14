using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages
{
    public class WeddingPartyModel : PageModel
    {
        private readonly ILogger<WeddingPartyModel> _logger;

        public WeddingPartyModel(ILogger<WeddingPartyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}