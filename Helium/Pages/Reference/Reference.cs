namespace Helium.Pages.Reference
{
    public class ReferenceEntry
    {
        public string Name { get; set; }
        public string Link { get; set; }

        public string MoreDetails { get; set; }

        public string MoreDetailsLink { get; set; }

        public ReferenceEntry(string name, string link, string moreDetails = "", string moreDetailsLink = "")
        {
            this.Name = name;
            this.Link = link;
            this.MoreDetails = moreDetails;
            this.MoreDetailsLink = moreDetailsLink;
        }
    }

    public class Reference
    {
        public string Title { get; set; }

        public List<ReferenceEntry> Entries { get; set; }
        public bool AddRow { get; set; }

        public Reference(string title, List<ReferenceEntry> entries, bool addRow = false)
        {
            this.Title = title;
            this.Entries = entries;
            this.AddRow = addRow;
        }
    }
}
