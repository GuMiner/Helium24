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

        public static Card AlgorithmAssistantModelCard = new Card(
            Area, "Algorithm Assistant", "IoTProjects/AlgorithmAssistant", "/img/iot-projects/AlgorithmAssistant.png",
            new DateTime(2013, 12, 1), new[] { Tag.Mobile, Tag.Software });
        public static Card CncMillSoftwareModelCard = new Card(
            Area, "Pro CNC 3020 Software", "IoTProjects/CncMillSoftware", "/img/iot-projects/CncMillSoftware.png",
            new DateTime(2015, 5, 1), new[] { Tag.LaserMill, Tag.Software });
        public static Card SpecialistsCalculatorModelCard = new Card(
            Area, "Specialists Calculator", "IoTProjects/SpecialistsCalculator", "/img/iot-projects/SpecialistsCalculator.png",
            new DateTime(2014, 8, 1), new[] { Tag.Mobile, Tag.Software });
        public static Card SkiGpsModelCard = new Card(
            Area, "Ski GPS Traces", "IoTProjects/SkiGps", "/img/iot-projects/SkiGpsTraces.png",
            new DateTime(2019, 1, 1), new[] { Tag.Design, Tag.Software });

        // PDA
        public static Card DraftingProgramModelCard = new Card(
            Area, "iPAQ PDA Drafting Program", "IoTProjects/PDA/DraftingProgram", "/img/iot-projects/pda/DraftingProgram.png",
            new DateTime(2011, 1, 1), new[] { Tag.Mobile, Tag.Software });
        public static Card FractalViewerModelCard = new Card(
            Area, "iPAQ PDA Fractal Viewer", "IoTProjects/PDA/FractalViewer", "/img/iot-projects/pda/FractalViewer.png",
            new DateTime(2011, 3, 1), new[] { Tag.Mobile, Tag.Software });
        public static Card LissajousCurvesModelCard = new Card(
            Area, "iPAQ PDA Lissajous Curves", "IoTProjects/PDA/LissajousCurves", "/img/iot-projects/pda/LissajousCurves.png",
            new DateTime(2010, 8, 1), new[] { Tag.Mobile, Tag.Software });
        public static Card ParticleColliderModelCard = new Card(
            Area, "iPAQ PDA Particle Collider", "IoTProjects/PDA/ParticleCollider", "/img/iot-projects/pda/ParticleCollider.png",
            new DateTime(2011, 7, 1), new[] { Tag.Mobile, Tag.Software });


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