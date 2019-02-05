using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages.Pcb
{
    public class MillBoardElectronicsModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/Pcb/MillBoardElectronics.png";

        public string Title => "Mill Board Electronics";

        public DateTime Date => new DateTime(2015, 2, 1);

        public Tag[] Tags => new[] { Tag.Hardware, Tag.LaserMill };

        public void OnGet()
        {
        }
    }
}
