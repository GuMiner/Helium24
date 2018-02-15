using System;

namespace Helium24.Models
{
    /// <summary>
    /// Defines the data necessary to display a thumbnail of a project.
    /// </summary>
    public class Project
    {
        public const string MarkdownSuffix = "md://";

        public Project(string thumbnailUri, string projectUri, string title, DateTime date, Tag[] tags)
        {
            this.ThumbnailUri = thumbnailUri;
            this.ProjectUri = projectUri;
            this.Title = title;
            this.Date = date;
            this.Tags = tags;
        }

        public string ThumbnailUri { get; }

        public string ProjectUri { get; private set; }

        public string Title { get; }

        public DateTime Date { get; }

        public Tag[] Tags { get; }

        internal string GetServerSideUri()
        {
            return $"/Markdown/{this.ProjectUri.Substring(Project.MarkdownSuffix.Length, this.ProjectUri.IndexOf('/', Project.MarkdownSuffix.Length) - Project.MarkdownSuffix.Length)}";
        }

        internal string GetMarkdownUri()
        {
            return this.ProjectUri.Substring(this.ProjectUri.IndexOf('/', MarkdownSuffix.Length) + 1);
        }

        internal Project WithoutMarkdownPrefix()
        {
            return new Project(this.ThumbnailUri, this.GetMarkdownUri(), this.Title, this.Date, this.Tags);
        }

        internal Project WithSimplifiedMarkdownUri()
        {
            return this.ProjectUri.StartsWith(MarkdownSuffix) ? new Project(this.ThumbnailUri, this.GetServerSideUri(), this.Title, this.Date, this.Tags) : this;
        }

        internal void RemoveMarkdownPrefix()
        {
            this.ProjectUri = this.GetMarkdownUri();
        }
    }
}
