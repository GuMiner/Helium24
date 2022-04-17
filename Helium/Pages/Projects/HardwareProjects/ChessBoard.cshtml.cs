using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.HardwareProjects
{
    public class ChessBoardModel : PageModel, ICard
    {
        public Card Card => HardwareProjectsModel.ChessBoardCard;

        public void OnGet()
        {

        }
    }
}