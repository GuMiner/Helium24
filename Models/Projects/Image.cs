namespace Helium24.Models
{
    /// <summary>
    /// Defines an image created for use in partial views.
    /// </summary>
    public class Image
    {
        public Image(string uri, string title)
        {
            this.Uri = uri;
            this.Title = title;
        }

        public string Uri { get; }

        public string Title { get; }
    }
}
