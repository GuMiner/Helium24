using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;

namespace Helium.Pages.Projects
{
    public class IoTProjectsModel : PageModel
    {
        private const string Area = "IoTProjects";

        public static List<Card> GitHubCards = new List<Card>()
        {
            new Card(Area, "SimpleSensor","https://github.com/GuMiner/garmin-simple-sensor", "/img/iot-projects/SimpleSensor.png",
                new DateTime(2018, 11, 10), new[] { Tag.Software }),
            new Card(Area, "SimpleClarity", "https://github.com/GuMiner/garmin-simple-clarity", "/img/iot-projects/SimpleClarity.png",
                new DateTime(2019, 1, 1), new[] { Tag.Software }),
            new Card(Area, "Playlist DJ", "https://github.com/GuMiner/playlist-dj", "/img/iot-projects/PlaylistDJ.png",
                new DateTime(2020, 8, 23), new[] { Tag.Software }),
            new Card(Area, "SuperTag", "https://github.com/GuMiner/SuperTag", "/img/iot-projects/SuperTag.png",
                new DateTime(2021, 3, 22), new[] { Tag.Software, Tag.Electronics }),
        };

        public static List<Card> Cards = typeof(IoTProjectsModel).GetFields(BindingFlags.Static | BindingFlags.Public)
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