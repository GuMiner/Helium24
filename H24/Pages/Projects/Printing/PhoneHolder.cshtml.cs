using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages.Printing
{
    public class PhoneHolderModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/3DPrinting/PhoneHolder.png";

        public string Title => "Phone Holder";

        public DateTime Date => new DateTime(2016, 4, 1);

        public Tag[] Tags => new[] { Tag.Hardware, Tag.Printer };

        public void OnGet()
        {
        }
    }
}
