using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages
{
    public class CncMillSoftwareModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/CncMillSoftware.png";

        public string Title => "Pro CNC 3020 Software";

        public DateTime Date => new DateTime(2015, 5, 1);

        public Tag[] Tags => new[] { Tag.Software, Tag.Hardware, Tag.LaserMill };

        public void OnGet()
        {
        }
    }
}
