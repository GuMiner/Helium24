using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.HardwareProjects.Printing
{
    public class PrintrbotAboutModel : PageModel, ICard
    {
        public Card Card => HardwareProjectsModel.PrintrbotAboutModelCard;
        public void OnGet()
        {

        }
    }
}