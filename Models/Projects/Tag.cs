namespace Helium24.Models
{
    /// <summary>
    /// Defines a tag for projects
    /// </summary>
    public class Tag
    {
        public static Tag Hardware { get; } = new Tag("Hardware", "orange");
            public static Tag LaserMill { get; } = new Tag("Laser / Mill", "red");
            public static Tag Printer { get; } = new Tag("3D Printer", "chartreuse");
        public static Tag Electronics { get; } = new Tag("Electronics", "olivedrab");

        public static Tag Software { get; } = new Tag("Software", "lightblue");
            public static Tag Games { get; } = new Tag("Games", "lightgreen");
            public static Tag Simulation { get; } = new Tag("Simulation", "magenta");

        public Tag(string name, string htmlColor)
        {
            this.Name = name;
            this.HtmlColor = htmlColor;
        }

        public string Name { get; }

        public string HtmlColor { get; }
    }
}
