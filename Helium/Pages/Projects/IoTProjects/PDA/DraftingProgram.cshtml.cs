using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.IoTProjects.PDA
{
    public class DraftingProgramModel : PageModel, ICard
    {
        public Card Card => IoTProjectsModel.DraftingProgramModelCard;
        public void OnGet()
        {

        }
    }
}