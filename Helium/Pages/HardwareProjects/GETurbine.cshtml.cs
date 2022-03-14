using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.HardwareProjects
{
    public class GETurbineModel : PageModel, ICard
    {
        public Card Card => HardwareProjectsModel.GETurbineCard;
        public void OnGet()
        {

        }
    }
}