using Newtonsoft.Json;
using System;

namespace H24.Models
{
    public class ProjectWithUri : IProjectWithUri
    {
        public string ProjectUri { get; set; }

        public string ThumbnailUri { get; set; }

        public string Title { get; set; }

        public DateTime Date { get; set; }

        [JsonConverter(typeof(TagConverter))]
        public Tag[] Tags { get; set; }

        [JsonConstructor]
        public ProjectWithUri()
        {
        }

        public ProjectWithUri(IProject project, string projectUri)
        {
            this.ProjectUri = projectUri;
            this.ThumbnailUri = project.ThumbnailUri;
            this.Title = project.Title;
            this.Date = project.Date;
            this.Tags = project.Tags;
        }
    }
}