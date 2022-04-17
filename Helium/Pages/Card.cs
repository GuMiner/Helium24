using Helium.Pages.Projects.Shared;

namespace Helium.Pages
{
    public class Card
    {
        public string Area { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }

        public List<Tag> Tags { get; set; }

        public Card(string area, string title, string link, string image, DateTime date, IEnumerable<Tag> tags)
        {
            Area = area;
            Title = title;
            Link = link;
            Image = image;
            Date = date;
            Tags = tags.ToList();
        }
    }
}
