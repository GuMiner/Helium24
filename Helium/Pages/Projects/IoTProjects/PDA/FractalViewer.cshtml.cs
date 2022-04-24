using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.IoTProjects.PDA
{
    public class FractalViewerModel : PageModel, ICard
    {
        public Card Card => IoTProjectsModel.FractalViewerModelCard;
        public void OnGet()
        {

        }
    }
}