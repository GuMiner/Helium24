using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.SimulationProjects
{
    public class VectorFlowModel : PageModel, ICard
    {
        public Card Card => SimulationProjectsModel.VectorFlowModelCard;
        public void OnGet()
        {

        }
    }
}