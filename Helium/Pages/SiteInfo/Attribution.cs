﻿namespace Helium.Pages.SiteInfo
{
    public class Attribution
    {
        public Attribution(string name, string uri, string usage)
        {
            this.Name = name;
            this.Uri = uri;
            this.Usage = usage;
        }

        public string Name { get; }

        public string Uri { get; }

        public string Usage { get; }
    }
}
