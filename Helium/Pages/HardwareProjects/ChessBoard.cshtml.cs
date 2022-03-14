using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.HardwareProjects
{
    public class ChessBoardModel : PageModel, ICard
    {
        public Card Card => HardwareProjectsModel.ChessBoardCard;

        public void OnGet()
        {

        }
    }
}