using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.SimulationProjects
{
    public class FluxSimModel : PageModel, ICard
    {
        public Card Card => SimulationProjectsModel.FluxSimModelCard;
        public void OnGet()
        {

        }
    }
}