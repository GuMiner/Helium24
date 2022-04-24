using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.SimulationProjects
{
    public class SimulatorModel : PageModel, ICard
    {
        public Card Card => SimulationProjectsModel.SimulatorModelCard;
        public void OnGet()
        {

        }
    }
}