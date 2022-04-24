namespace Helium.Pages.Projects.Shared
{
    public class Tag
    {
        public static Tag LaserMill = new Tag("Laser / Mill", "red");
        public static Tag Printer = new Tag("3D Printer", "chartreuse");
        public static Tag Electronics = new Tag("Electronics", "olivedrab");
        public static Tag Software = new Tag("Software", "lightblue");
        public static Tag Mobile = new Tag("Mobile", "slateblue");
        public static Tag Games = new Tag("Games", "lightgreen");
        public static Tag Simulation = new Tag("Simulation", "magenta");
        public static Tag Design = new Tag("Design", "salmon");

        public Tag(string name, string color)
        {
            this.Name = name;
            this.Color = color;
        }

        public string Name { get; }

        public string Color { get; }

        public static Tag Parse(string tagName)
        {
            switch (tagName)
            {
                case nameof(Design): return Design;
                case nameof(LaserMill): return LaserMill;
                case nameof(Printer): return Printer;
                case nameof(Electronics): return Electronics;
                case nameof(Software): return Software;
                case nameof(Mobile): return Mobile;
                case nameof(Games): return Games;
                case nameof(Simulation): return Simulation;
                default:
                    throw new Exception($"Unexpected tag name {tagName}");
            }
        }
    }
}
