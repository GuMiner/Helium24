using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.HardwareProjects
{
    public class GETurbineModel : PageModel, ICard
    {
        public Card Card => HardwareProjectsModel.GETurbineCard;
        public void OnGet()
        {

        }
    }
}