using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages.Printing.Cases
{
    public class GalileoCaseModel : PageModel, IProject
    {
        public string ThumbnailUri => "Blobs/ProjectThumbnails/3DPrinting/Cases/GalileoCase.png";

        public string Title => "Intel Galileo Gen 2 Case";

        public DateTime Date => new DateTime(2014, 8, 1);

        public Tag[] Tags => new[] { Tag.Hardware, Tag.Printer };

        public void OnGet()
        {
        }
    }
}
