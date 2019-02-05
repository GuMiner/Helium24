using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages.Printing
{
    public class PrintrbotAboutModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/3DPrinting/PrintrbotAbout.png";

        public string Title => "About the Printrbot Jr";

        public DateTime Date => new DateTime(2013, 7, 1);

        public Tag[] Tags => new[] { Tag.Hardware, Tag.Printer, Tag.Electronics };

        public void OnGet()
        {
        }
    }
}
