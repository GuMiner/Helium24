namespace Helium24.Definitions
{
    /// <summary>
    /// Defines a tag for projects
    /// </summary>
    public class Tag
    {
        public static Tag Hardware { get; } = new Tag("Hardware", "orange");
        public static Tag Software { get; } = new Tag("Software", "lightblue");
        public static Tag Laser { get; } = new Tag("Laser", "red");
        public static Tag Games { get; } = new Tag("Games", "lightgreen");

        public Tag(string name, string htmlColor)
        {
            this.Name = name;
            this.HtmlColor = htmlColor;
        }

        public string Name { get; }

        public string HtmlColor { get; }
    }
}
