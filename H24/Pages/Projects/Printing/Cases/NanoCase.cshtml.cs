using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages.Printing.Cases
{
    public class NanoCaseModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/3DPrinting/Cases/NanoCase.png";

        public string Title => "Arduino Nano Case";

        public DateTime Date => new DateTime(2015, 11, 1);

        public Tag[] Tags => new[] { Tag.Hardware, Tag.Printer };

        public void OnGet()
        {
        }
    }
}
