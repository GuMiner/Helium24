using Helium24.Models;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Helium24.Definitions
{
    public class SerializedProject
    {
        [JsonConstructor]
        public SerializedProject(string thumbnailUri, string projectUri, string title, DateTime date, WellKnownTag[] tags)
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

        public WellKnownTag[] Tags { get; }

        public static explicit operator Project (SerializedProject project)
        {
            return new Project(project.ThumbnailUri, project.ProjectUri, project.Title,
                project.Date, project.Tags.Select(tag => Tag.ConvertFromWellKnownTag(tag)).ToArray());
        }
    }
}
