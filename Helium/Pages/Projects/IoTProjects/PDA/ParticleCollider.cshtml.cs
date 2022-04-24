using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.IoTProjects.PDA
{
    public class ParticleColliderModel : PageModel, ICard
    {
        public Card Card => IoTProjectsModel.ParticleColliderModelCard;
        public void OnGet()
        {

        }
    }
}