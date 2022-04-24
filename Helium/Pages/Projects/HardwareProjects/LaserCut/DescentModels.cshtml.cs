using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.HardwareProjects.LaserCut
{
    public class DescentModelsModel : PageModel, ICard
    {
        public Card Card => HardwareProjectsModel.DescentModelsCard;

        public void OnGet()
        {

        }
    }
}