using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.HardwareProjects.LaserCut
{
    public class FilamentHolderModel : PageModel, ICard
    {
        public Card Card => HardwareProjectsModel.FilamentHolderCard;

        public void OnGet()
        {

        }
    }
}