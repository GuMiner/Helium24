using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.HardwareProjects.Printing
{
    public class PhoneHolderModel : PageModel, ICard
    {
        public Card Card => HardwareProjectsModel.PhoneHolderModelCard;
        public void OnGet()
        {

        }
    }
}