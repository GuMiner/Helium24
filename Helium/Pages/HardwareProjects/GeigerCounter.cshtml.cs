using Helium.Pages.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.HardwareProjects
{
    public class GeigerCounterModel : PageModel, ICard
    {
        public Card Card => HardwareProjectsModel.GeigerCounterCard;

        public void OnGet()
        {
        }
    }
}