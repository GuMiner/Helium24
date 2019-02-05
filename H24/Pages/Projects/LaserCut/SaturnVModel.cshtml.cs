using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages.LaserCut
{
    public class SaturnVModelModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/LaserCut/SaturnVModel.png";

        public string Title => "Saturn V Model";

        public DateTime Date => new DateTime(2014, 10, 1);

        public Tag[] Tags => new[] { Tag.Hardware, Tag.LaserMill };

        public void OnGet()
        {
        }
    }
}
