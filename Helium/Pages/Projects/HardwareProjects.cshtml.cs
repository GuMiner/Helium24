using Helium.Pages.Projects.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;

namespace Helium.Pages.Projects
{
    public class HardwareProjectsModel : PageModel
    {
        private const string Area = "HardwareProjects";
        public static Card GeigerCounterCard = new Card(
            Area, "DIY Geiger Counter", "HardwareProjects/GeigerCounter", "/img/hardware-projects/GeigerCounter.png",
            new DateTime(2013, 11, 1), new[] { Tag.Printer, Tag.Electronics });
        public static Card MillBoardElectronicsModelCard = new Card(
            Area, "Mill Board Electronics", "HardwareProjects/MillBoardElectronics", "/img/hardware-projects/MillBoardElectronics.png",
            new DateTime(2015, 2, 1), new[] { Tag.LaserMill, Tag.Electronics });

        // Laser Cut
        public static Card ChessBoardCard = new Card( 
            Area, "Laser-engraved chess board", "HardwareProjects/LaserCut/ChessBoard", "/img/hardware-projects/laser-cut/ChessBoard.png",
            new DateTime(2015, 4, 1), new[] { Tag.LaserMill, Tag.Games });
        public static Card DescentModelsCard = new Card(
            Area, "Descent Laser Models", "HardwareProjects/LaserCut/DescentModels", "/img/hardware-projects/laser-cut/DescentModels.png",
            new DateTime(2014, 8, 1), new[] { Tag.LaserMill });
        public static Card FilamentHolderCard = new Card(
            Area, "Filament Holder", "HardwareProjects/LaserCut/FilamentHolder", "/img/hardware-projects/laser-cut/FilamentHolder.png",
            new DateTime(2014, 7, 1), new[] { Tag.LaserMill });
        public static Card SaturnVModelCard = new Card(
            Area, "Saturn V Model", "HardwareProjects/LaserCut/SaturnVModel", "/img/hardware-projects/laser-cut/SaturnVModel.png",
            new DateTime(2014, 10, 1), new[] { Tag.LaserMill });
        public static Card SpaceShuttleModelCard = new Card(
            Area, "Space Shuttle Model", "HardwareProjects/LaserCut/SpaceShuttleModel", "/img/hardware-projects/laser-cut/SpaceShuttleModel.png",
            new DateTime(2014, 7, 1), new[] { Tag.LaserMill });
        public static Card WaveGeneratorCard = new Card(
            Area, "Wave Generator", "HardwareProjects/LaserCut/WaveGenerator", "/img/hardware-projects/laser-cut/WaveGenerator.png",
            new DateTime(2014, 7, 1), new[] { Tag.LaserMill });

        // 3D Printed
        public static Card PrintrbotAboutModelCard = new Card(
            Area, "About the Printrbot Jr", "HardwareProjects/Printing/PrintrbotAbout", "/img/hardware-projects/printing/PrintrbotAbout.png",
            new DateTime(2013, 7, 1), new[] { Tag.Printer, Tag.Electronics });

        public static Card GETurbineCard = new Card(
            Area, "GE Turbine Model", "HardwareProjects/Printing/GETurbine", "/img/hardware-projects/printing/GETurbine.png",
            new DateTime(2016, 1, 1), new[] { Tag.Printer });
        public static Card N64LogoModelCard = new Card(
            Area, "Nintendo 64 Logo", "HardwareProjects/Printing/N64Logo", "/img/hardware-projects/printing/N64Logo.png",
            new DateTime(2016, 2, 1), new[] { Tag.Printer });
        public static Card GearHolderModelCard = new Card(
            Area, "Gear Holder", "HardwareProjects/Printing/GearHolder", "/img/hardware-projects/printing/GearHolder.png",
            new DateTime(2015, 12, 1), new[] { Tag.Printer });
        public static Card PhoneHolderModelCard = new Card(
            Area, "Phone Holder", "HardwareProjects/Printing/PhoneHolder", "/img/hardware-projects/printing/PhoneHolder.png",
            new DateTime(2016, 4, 1), new[] { Tag.Printer });

        // 3D Printed - Cases
        public static Card GalileoCaseModelCard = new Card(
            Area, "Intel Galileo Gen 2 Case", "HardwareProjects/Printing/Cases/GalileoCase", "/img/hardware-projects/printing/cases/GalileoCase.png",
            new DateTime(2014, 8, 1), new[] { Tag.Printer });
        public static Card NanoCaseModelCard = new Card(
            Area, "Arduino Nano Case", "HardwareProjects/Printing/Cases/NanoCase", "/img/hardware-projects/printing/cases/NanoCase.png",
            new DateTime(2015, 11, 1), new[] { Tag.Printer });
        public static Card PiCaseModelCard = new Card(
            Area, "Raspberry Pi Case", "HardwareProjects/Printing/Cases/PiCase", "/img/hardware-projects/printing/cases/PiCase.png",
            new DateTime(2015, 10, 1), new[] { Tag.Printer });
        public static Card UtilityCasesModelCard = new Card(
            Area, "Utility Cases", "HardwareProjects/Printing/Cases/UtilityCases", "/img/hardware-projects/printing/cases/UtilityCases.png",
            new DateTime(2014, 3, 1), new[] { Tag.Printer });

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