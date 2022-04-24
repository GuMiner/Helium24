using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.IoTProjects
{
    public class CncMillSoftwareModel : PageModel, ICard
    {
        public Card Card => IoTProjectsModel.CncMillSoftwareModelCard;
        public void OnGet()
        {

        }
    }
}