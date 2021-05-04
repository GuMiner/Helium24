namespace H24.Models
{
    /// <summary>
    /// Defines an attribution created for use in view creation.
    /// </summary>
    public class Attribution
    {
        public Attribution(string name, string uri, string usage)
        {
            this.Name = name;
            this.Uri = uri;
            this.Usage = usage;
        }

        public string Name { get; }

        public string Uri { get; }

        public string Usage { get; }
    }
}
