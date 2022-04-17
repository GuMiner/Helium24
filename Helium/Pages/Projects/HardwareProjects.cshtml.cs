using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;

namespace Helium.Pages.Projects
{
    public class HardwareProjectsModel : PageModel
    {
        private const string Area = "HardwareProjects";
        public static Card GeigerCounterCard = new Card(Area, "DIY Geiger Counter", "HardwareProjects/GeigerCounter", "/img/hardware-projects/GeigerCounter.png",
                new DateTime(2013, 11, 1), new[] { Tag.Printer, Tag.Electronics });
        public static Card GETurbineCard = new Card( 
            Area, "GE Turbine Model", "HardwareProjects/GETurbine", "/img/hardware-projects/GETurbine.png",
            new DateTime(2016, 1, 1), new[] { Tag.Printer });
        public static Card ChessBoardCard = new Card( 
            Area, "Laser-engraved chess board", "HardwareProjects/ChessBoard", "/img/hardware-projects/ChessBoard.png",
            new DateTime(2015, 4, 1), new[] { Tag.LaserMill, Tag.Games });

        public static List<Card> GitHubCards = new List<Card>()
        {
        };

        public static List<Card> Cards = typeof(HardwareProjectsModel).GetFields(BindingFlags.Static | BindingFlags.Public)
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