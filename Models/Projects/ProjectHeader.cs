namespace Helium24.Models
{
    /// <summary>
    /// Defines a common header format for projects.
    /// </summary>
    public class ProjectHeader
    {
        public ProjectHeader(string title, string date, Tag[] tags)
        {
            this.Title = title;
            this.Date = date;
            this.Tags = tags;
        }

        public string Title { get; }

        public string Date { get; }

        public Tag[] Tags { get; }
    }
}
