namespace H24.Models
{
    /// <summary>
    /// Defines a tag for projects
    /// </summary>
    public class Tag
    {
        public static Tag Hardware = new Tag("Hardware", "orange");
        public static Tag LaserMill = new Tag("Laser / Mill", "red");
        public static Tag Printer = new Tag("3D Printer", "chartreuse");
        public static Tag Electronics = new Tag("Electronics", "olivedrab");
        public static Tag Software = new Tag("Software", "lightblue");
        public static Tag Mobile = new Tag("Mobile", "slateblue");
        public static Tag Games = new Tag("Games", "lightgreen");
        public static Tag Simulation = new Tag("Simulation", "magenta");

        public Tag(string name, string htmlColor)
        {
            this.Name = name;
            this.HtmlColor = htmlColor;
        }

        public string Name { get; }

        public string HtmlColor { get; }

        public static Tag Parse(string tagName)
        {
            switch(tagName)
            {
                case nameof(Hardware): return Hardware;
                case nameof(LaserMill): return LaserMill;
                case nameof(Printer): return Printer;
                case nameof(Electronics): return Electronics;
                case nameof(Software): return Software;
                case nameof(Mobile): return Mobile;
                case nameof(Games): return Games;
                case nameof(Simulation): return Simulation;
                default:
                    return null;
            }
        }
    }
}