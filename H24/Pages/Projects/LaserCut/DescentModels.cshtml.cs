using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages.LaserCut
{
    public class DescentModelsModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/LaserCut/DescentModels.png";

        public string Title => "Descent Models";

        public DateTime Date => new DateTime(2014, 8, 1);

        public Tag[] Tags => new[] { Tag.Hardware, Tag.LaserMill };

        public void OnGet()
        {
        }
    }
}
