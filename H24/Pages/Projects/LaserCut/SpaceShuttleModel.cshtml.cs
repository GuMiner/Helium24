using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages.LaserCut
{
    public class SpaceShuttleModelModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/LaserCut/SpaceShuttleModel.png";

        public string Title => "Space Shuttle Model";

        public DateTime Date => new DateTime(2014, 7, 1);

        public Tag[] Tags => new[] { Tag.Hardware, Tag.LaserMill };

        public void OnGet()
        {
        }
    }
}
