using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.HardwareProjects
{
    public class GeigerCounterModel : PageModel, ICard
    {
        public Card Card => HardwareProjectsModel.GeigerCounterCard;

        public void OnGet()
        {
        }
    }
}