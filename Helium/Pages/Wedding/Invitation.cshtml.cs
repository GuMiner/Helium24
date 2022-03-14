using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages
{
    public class InvitationModel : PageModel
    {
        private readonly ILogger<InvitationModel> _logger;

        public InvitationModel(ILogger<InvitationModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}