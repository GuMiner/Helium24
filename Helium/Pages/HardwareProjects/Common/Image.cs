namespace Helium.Pages.HardwareProjects.Common
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
