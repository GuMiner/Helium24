using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;

namespace Helium.Pages.Projects
{
    public class SoftwareProjectsModel : PageModel
    {
        private const string Area = "SoftwareProjects";

        public static List<Card> GitHubCards = new List<Card>()
        {
            new Card(Area, "Octocad", "https://github.com/GuMiner/Octocad", "/img/software-projects/Octocad.png",
                new DateTime(2016, 11, 1), new[] { Tag.Software }),
            new Card(Area, "Napack Package Framework", "https://github.com/GuMiner/napack", "/img/software-projects/Napack.png",
                new DateTime(2017, 2, 1), new[] { Tag.Software }),
            new Card(Area, "RainBot","https://github.com/GuMiner/RainBot", "/img/software-projects/RainBot.png",
                new DateTime(2017, 10, 1), new[] { Tag.Software }),
            new Card(Area, "PieBot", "https://github.com/GuMiner/PieBot", "/img/software-projects/PieBot.png",
                new DateTime(2017, 11, 1), new[] { Tag.Software }),
            new Card(Area, "GeoWeather API", "https://github.com/GuMiner/GeoWeather", "/img/software-projects/GeoWeather.png",
                new DateTime(2018, 1, 1), new[] { Tag.Software }),
            new Card(Area, "Topography Rasterization", "https://github.com/GuMiner/TopographicRasterizer", "/img/software-projects/TopographicRasterizer.png",
                new DateTime(2018, 12, 1), new[] { Tag.Design, Tag.Printer, Tag.Software }),
        };


        public static Card CodeGellModelCard = new Card(
            Area, "CodeGell", "SoftwareProjects/CodeGell", "/img/software-projects/CodeGell.png",
            new DateTime(2014, 6, 1), new[] { Tag.Games, Tag.Software });
        public static Card CourseDegreeTrackerModelCard = new Card(
            Area, "Course Degree Tracker", "SoftwareProjects/CourseDegreeTracker", "/img/software-projects/CourseDegreeTracker.png",
            new DateTime(2012, 2, 1), new[] { Tag.Software });
        public static Card QuantumComputingModelCard = new Card(
            Area, "Quantum Computing", "SoftwareProjects/QuantumComputing", "/img/software-projects/QuantumComputing.png",
            new DateTime(2018, 2, 1), new[] { Tag.Software, Tag.Simulation });

        public static List<Card> Cards = typeof(SoftwareProjectsModel).GetFields(BindingFlags.Static | BindingFlags.Public)
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