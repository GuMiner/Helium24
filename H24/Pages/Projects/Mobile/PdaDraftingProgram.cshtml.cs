using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages.Mobile
{
    public class PdaDraftingProgramModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/Mobile/PdaDraftingProgram.png";

        public string Title => "iPAQ PDA Drafting Program";

        public DateTime Date => new DateTime(2011, 1, 1);

        public Tag[] Tags => new[] { Tag.Software, Tag.Mobile };

        public void OnGet()
        {
        }
    }
}
