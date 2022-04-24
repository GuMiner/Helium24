using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.HardwareProjects.LaserCut
{
    public class SaturnVModelModel : PageModel, ICard
    {
        public Card Card => HardwareProjectsModel.SaturnVModelCard;

        public void OnGet()
        {

        }
    }
}