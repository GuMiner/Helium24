using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.SimulationProjects
{
    public class FieldSimulatorModel : PageModel, ICard
    {
        public Card Card => SimulationProjectsModel.FieldSimulatorModelCard;
        public void OnGet()
        {

        }
    }
}