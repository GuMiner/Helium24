using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.HardwareProjects.Printing
{
    public class GearHolderModel : PageModel, ICard
    {
        public Card Card => HardwareProjectsModel.GearHolderModelCard;
        public void OnGet()
        {

        }
    }
}