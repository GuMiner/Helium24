using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages.Printing.Cases
{
    public class PiCaseModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/3DPrinting/Cases/PiCase.png";

        public string Title => "Raspberry Pi Case";

        public DateTime Date => new DateTime(2015, 10, 1);

        public Tag[] Tags => new[] { Tag.Hardware, Tag.Printer };

        public void OnGet()
        {
        }
    }
}
