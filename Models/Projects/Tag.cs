namespace Helium24.Models
{
    /// <summary>
    /// Defines a tag for projects
    /// </summary>
    public class Tag
    {
        public Tag(string name, string htmlColor)
        {
            this.Name = name;
            this.HtmlColor = htmlColor;
        }

        public string Name { get; }

        public string HtmlColor { get; }

        public static Tag ConvertFromWellKnownTag(WellKnownTag wellKnownTag)
        {
            switch (wellKnownTag)
            {
                case WellKnownTag.Hardware:
                    return new Tag("Hardware", "orange");
                case WellKnownTag.LaserMill:
                    return new Tag("Laser / Mill", "red");
                case WellKnownTag.Printer:
                    return new Tag("3D Printer", "chartreuse");
                case WellKnownTag.Electronics:
                    return new Tag("Electronics", "olivedrab");
                case WellKnownTag.Software:
                    return new Tag("Software", "lightblue");
                case WellKnownTag.Mobile:
                    return new Tag("Mobile", "slateblue");
                case WellKnownTag.Games:
                    return new Tag("Games", "lightgreen");
                case WellKnownTag.Simulation:
                    return new Tag("Simulation", "magenta");
                case WellKnownTag.None:
                default:
                    return new Models.Tag("Unknown", "brown");
        }
    }
}
}
