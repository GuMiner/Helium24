using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.HardwareProjects.LaserCut
{
    public class SpaceShuttleModel : PageModel, ICard
    {
        public Card Card => HardwareProjectsModel.SpaceShuttleModelCard;

        public void OnGet()
        {

        }
    }
}