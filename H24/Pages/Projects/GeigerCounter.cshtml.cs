using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages
{
    public class GeigerCounterModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/GeigerCounter.png";

        public string Title => "Geiger Counter";

        public DateTime Date => new DateTime(2013, 11, 1);

        public Tag[] Tags => new[] { Tag.Hardware, Tag.Printer, Tag.Electronics };

        public void OnGet()
        {
        }
    }
}
