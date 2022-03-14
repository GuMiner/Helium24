using Helium.Pages.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;

namespace Helium.Pages
{
    public class HardwareProjectsModel : PageModel
    {
        public static Card GeigerCounterCard = new Card("DIY Geiger Counter", "/HardwareProjects/GeigerCounter", "/img/hardware-projects/GeigerCounter.png",
                new DateTime(2013, 11, 1), new[] { Tag.Printer, Tag.Electronics });
        public static Card GETurbineCard = new Card(
            "GE Turbine Model", "/HardwareProjects/GETurbine", "img/hardware-projects/GETurbine.png",
            new DateTime(2016, 1, 1), new[] { Tag.Printer });
        public static Card ChessBoardCard = new Card(
            "Laser-engraved chess board", "/HardwareProjects/ChessBoard", "img/hardware-projects/ChessBoard.png",
            new DateTime(2015, 4, 1), new[] { Tag.LaserMill, Tag.Games });

        public static List<Card> GitHubCards = new List<Card>()
        {
            new Card("Asteroida Graphica", "https://github.com/GuMiner/AsteroidaGraphica", "img/hardware-projects/AsteroidaGraphica.png",
                new DateTime(2015, 12, 01), new[] { Tag.Software, Tag.Games }),
            new Card("Temper Fine", "https://github.com/GuMiner/TemperFine", "img/hardware-projects/TemperFine.png",
                new DateTime(2016, 02, 01), new[] { Tag.Software, Tag.Games }),
            new Card("Octocad", "https://github.com/GuMiner/Octocad", "img/hardware-projects/Octocad.png",
                new DateTime(2016, 11, 1), new[] { Tag.Software }),
            new Card("Napack Package Framework", "https://github.com/GuMiner/napack", "img/hardware-projects/Napack.png",
                new DateTime(2017, 2, 1), new[] { Tag.Software }),
            new Card("agow", "https://github.com/GuMiner/agow", "img/hardware-projects/Agow.png",
                new DateTime(2017, 3, 1), new[] { Tag.Software, Tag.Games }),
            new Card("C# Adventure API", "https://github.com/GuMiner/Adventure", "img/hardware-projects/AdventurePort.png",
                new DateTime(2017, 10, 1), new[] { Tag.Software }),
            new Card("RainBot","https://github.com/GuMiner/RainBot", "img/hardware-projects/RainBot.png",
                new DateTime(2017, 10, 1), new[] { Tag.Software }),
            new Card("PieBot", "https://github.com/GuMiner/PieBot", "img/hardware-projects/PieBot.png",
                new DateTime(2017, 11, 1), new[] { Tag.Software }),
            new Card("GeoWeather API", "https://github.com/GuMiner/GeoWeather", "img/hardware-projects/GeoWeather.png",
                new DateTime(2018, 1, 1), new[] { Tag.Software }),
            new Card("SimpleSensor","https://github.com/GuMiner/garmin-simple-sensor", "img/hardware-projects/SimpleSensor.png",
                new DateTime(2018, 11, 10), new[] { Tag.Software }),
            new Card("Topography Rasterization", "https://github.com/GuMiner/TopographicRasterizer", "img/hardware-projects/TopographicRasterizer.png",
                new DateTime(2018, 12, 1), new[] { Tag.Design, Tag.Printer, Tag.Software }),
            new Card("SimpleClarity", "https://github.com/GuMiner/garmin-simple-clarity", "img/hardware-projects/SimpleClarity.png",
                new DateTime(2019, 1, 1), new[] { Tag.Software }),
            new Card("Playlist DJ", "https://github.com/GuMiner/playlist-dj", "img/hardware-projects/PlaylistDJ.png",
                new DateTime(2020, 8, 23), new[] { Tag.Software }),
            new Card("DPC++ Experiments", "https://github.com/GuMiner/DPC-experiments", "img/hardware-projects/DPCExperiments.png",
                new DateTime(2021, 2, 1), new[] { Tag.Software, Tag.Simulation }),
            new Card("SuperTag", "https://github.com/GuMiner/SuperTag", "img/hardware-projects/SuperTag.png",
                new DateTime(2021, 3, 22), new[] { Tag.Software, Tag.Electronics }),
        };

        public static List<Card> Cards = typeof(HardwareProjectsModel).GetFields(BindingFlags.Static | BindingFlags.Public)
            .Where(field => field.FieldType == typeof(Card)).Select(field => field.GetValue(null) as Card).Select(card => card!)
            .Union(GitHubCards)
            .OrderByDescending(card => card.Date)
            .ToList();

        public void OnGet()
        {

        }
    }
}