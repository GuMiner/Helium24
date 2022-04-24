using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.IoTProjects
{
    public class AlgorithmAssistantModel : PageModel, ICard
    {
        public Card Card => IoTProjectsModel.AlgorithmAssistantModelCard;
        public void OnGet()
        {

        }
    }
}