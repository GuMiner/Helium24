using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.HardwareProjects.Printing
{
    public class N64LogoModel : PageModel, ICard
    {
        public Card Card => HardwareProjectsModel.N64LogoModelCard;
        public void OnGet()
        {

        }
    }
}
