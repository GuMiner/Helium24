using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;

namespace Helium.Pages.Projects
{
    public class GameProjectsModel : PageModel
    {
        private const string Area = "GameProjects";

        public static List<Card> GitHubCards = new List<Card>()
        {
            new Card(Area, "Asteroida Graphica", "https://github.com/GuMiner/AsteroidaGraphica", "/img/game-projects/AsteroidaGraphica.png",
                new DateTime(2015, 12, 01), new[] { Tag.Software, Tag.Games }),
            new Card(Area, "Temper Fine", "https://github.com/GuMiner/TemperFine", "/img/game-projects/TemperFine.png",
                new DateTime(2016, 02, 01), new[] { Tag.Software, Tag.Games }),
            new Card(Area, "agow", "https://github.com/GuMiner/agow", "/img/game-projects/Agow.png",
                new DateTime(2017, 3, 1), new[] { Tag.Software, Tag.Games }),
            new Card(Area, "C# Adventure API", "https://github.com/GuMiner/Adventure", "/img/game-projects/AdventurePort.png",
                new DateTime(2017, 10, 1), new[] { Tag.Software }),
        };

        public static List<Card> Cards = typeof(GameProjectsModel).GetFields(BindingFlags.Static | BindingFlags.Public)
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