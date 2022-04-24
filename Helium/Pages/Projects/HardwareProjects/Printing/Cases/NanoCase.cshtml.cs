using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.HardwareProjects.Printing.Cases
{
    public class NanoCaseModel : PageModel, ICard
    {
        public Card Card => HardwareProjectsModel.NanoCaseModelCard;
        public void OnGet()
        {

        }
    }
}