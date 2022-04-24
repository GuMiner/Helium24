using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.IoTProjects.PDA
{
    public class LissajousCurvesModel : PageModel, ICard
    {
        public Card Card => IoTProjectsModel.LissajousCurvesModelCard;
        public void OnGet()
        {

        }
    }
}