using System;

namespace Helium24.Models
{
    /// <summary>
    /// Defines the data necessary to display a thumbnail of a project.
    /// </summary>
    public class ProjectThumbnail
    {
        public ProjectThumbnail(string thumbnailUri, string projectUri, string title, DateTime date, Tag[] tags)
        {
            this.ThumbnailUri = thumbnailUri;
            this.ProjectUri = projectUri;
            this.Title = title;
            this.Date = date;
            this.Tags = tags;
        }

        public string ThumbnailUri { get; }

        public string ProjectUri { get; }

        public string Title { get; }

        public DateTime Date { get; }

        public Tag[] Tags { get; }
    }
}
