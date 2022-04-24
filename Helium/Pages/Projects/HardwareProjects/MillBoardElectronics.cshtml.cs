using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.HardwareProjects
{
    public class MillBoardElectronicsModel : PageModel, ICard
    {
        public Card Card => HardwareProjectsModel.MillBoardElectronicsModelCard;
        public void OnGet()
        {

        }
    }
}