using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.SoftwareProjects
{
    public class QuantumComputingModel : PageModel, ICard
    {
        public Card Card => SoftwareProjectsModel.QuantumComputingModelCard;
        public void OnGet()
        {

        }
    }
}