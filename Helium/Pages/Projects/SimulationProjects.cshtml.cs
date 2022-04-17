using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;

namespace Helium.Pages.Projects
{
    public class SimulationProjectsModel : PageModel
    {
        private const string Area = "SimulationProjects";

        public static List<Card> GitHubCards = new List<Card>()
        {
            new Card(Area, "DPC++ Experiments", "https://github.com/GuMiner/DPC-experiments", "/img/simulation-projects/DPCExperiments.png",
                new DateTime(2021, 2, 1), new[] { Tag.Software, Tag.Simulation }),
        };

        public static List<Card> Cards = typeof(SimulationProjectsModel).GetFields(BindingFlags.Static | BindingFlags.Public)
            .Where(field => field.FieldType == typeof(Card)).Select(field => field.GetValue(null) as Card).Select(card => card!)
            .Where(card => card.Area.Equals(Area))
            .Union(GitHubCards)
            .OrderByDescending(card => card.Date)
            .ToList();

        public void OnGet()
        {

        }
    }
}