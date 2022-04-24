using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.IoTProjects
{
    public class SpecialistsCalculatorModel : PageModel, ICard
    {
        public Card Card => IoTProjectsModel.SpecialistsCalculatorModelCard;
        public void OnGet()
        {

        }
    }
}