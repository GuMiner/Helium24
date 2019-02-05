using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages.LaserCut
{
    public class FilamentHolderModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/LaserCut/FilamentHolder.png";

        public string Title => "Filament Holder";

        public DateTime Date => new DateTime(2014, 7, 1);

        public Tag[] Tags => new[] { Tag.Hardware, Tag.LaserMill };

        public void OnGet()
        {
        }
    }
}
