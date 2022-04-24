using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.IoTProjects
{
    public class SkiGpsModel : PageModel, ICard
    {
        public Card Card => IoTProjectsModel.SkiGpsModelCard;
        public void OnGet()
        {

        }
    }
}