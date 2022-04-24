using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.HardwareProjects.Printing.Cases
{
    public class PiCaseModel : PageModel, ICard
    {
        public Card Card => HardwareProjectsModel.PiCaseModelCard;
        public void OnGet()
        {

        }
    }
}