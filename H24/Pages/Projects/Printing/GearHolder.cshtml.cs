using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages.Printing
{
    public class GearHolderModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/3DPrinting/GearHolder.png";

        public string Title => "Gear Holder";

        public DateTime Date => new DateTime(2015, 12, 1);

        public Tag[] Tags => new[] { Tag.Hardware, Tag.Printer };

        public void OnGet()
        {
        }
    }
}
