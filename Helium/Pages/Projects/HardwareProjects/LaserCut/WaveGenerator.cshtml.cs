using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.HardwareProjects.LaserCut
{
    public class WaveGenerator : PageModel, ICard
    {
        public Card Card => HardwareProjectsModel.WaveGeneratorCard;

        public void OnGet()
        {

        }
    }
}