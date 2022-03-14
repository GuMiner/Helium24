using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages
{
    public class RegistryModel : PageModel
    {
        private readonly ILogger<InvitationModel> _logger;

        public RegistryModel(ILogger<InvitationModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}