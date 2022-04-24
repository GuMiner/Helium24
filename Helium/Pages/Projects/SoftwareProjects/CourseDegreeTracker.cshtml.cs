using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages.Projects.SoftwareProjects
{
    public class CourseDegreeTrackerModel : PageModel, ICard
    {
        public Card Card => SoftwareProjectsModel.CourseDegreeTrackerModelCard;
        public void OnGet()
        {

        }
    }
}