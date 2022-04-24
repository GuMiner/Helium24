using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.SimulationProjects
{
    public class BeamFlowModel : PageModel, ICard
    {
        public Card Card => SimulationProjectsModel.BeamFlowModelCard;
        public void OnGet()
        {

        }
    }
}