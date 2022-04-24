using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.HardwareProjects.Printing.Cases
{
    public class UtilityCasesModel : PageModel, ICard
    {
        public Card Card => HardwareProjectsModel.UtilityCasesModelCard;
        public void OnGet()
        {

        }
    }
}