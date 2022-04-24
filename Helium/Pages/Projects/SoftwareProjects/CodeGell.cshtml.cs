using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.SoftwareProjects
{
    public class CodeGellModel : PageModel, ICard
    {
        public Card Card => SoftwareProjectsModel.CodeGellModelCard;
        public void OnGet()
        {

        }
    }
}
