using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages
{
    public class AttributionsModel : PageModel
    {
        private readonly ILogger<AttributionsModel> _logger;

        public AttributionsModel(ILogger<AttributionsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}