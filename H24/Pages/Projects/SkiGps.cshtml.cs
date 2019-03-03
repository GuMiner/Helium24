using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages
{
    public class SkiGpsModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/SkiGpsTraces.png";

        public string Title => "Ski GPS Traces";

        public DateTime Date => new DateTime(2019, 1, 1);

        public Tag[] Tags => new[] { Tag.Design, Tag.Software };

        public void OnGet()
        {
        }
    }
}
