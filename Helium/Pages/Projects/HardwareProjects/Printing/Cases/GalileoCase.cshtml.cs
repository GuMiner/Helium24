using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.HardwareProjects.Printing.Cases
{
    public class GalileoCaseModel : PageModel, ICard
    {
        public Card Card => HardwareProjectsModel.GalileoCaseModelCard;
        public void OnGet()
        {

        }
    }
}