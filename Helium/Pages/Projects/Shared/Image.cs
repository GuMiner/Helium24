namespace Helium.Pages.Projects.Shared
{
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
