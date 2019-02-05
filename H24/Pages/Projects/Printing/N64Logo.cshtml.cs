using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages.Printing
{
    public class N64LogoModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/3DPrinting/N64Logo.png";

        public string Title => "Nintendo 64 Logo";

        public DateTime Date => new DateTime(2016, 2, 1);

        public Tag[] Tags => new[] { Tag.Hardware, Tag.Printer };

        public void OnGet()
        {
        }
    }
}
