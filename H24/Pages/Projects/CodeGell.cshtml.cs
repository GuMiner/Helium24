using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages
{
    public class CodeGellModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/CodeGell.png";

        public string Title => "CodeGell";

        public DateTime Date => new DateTime(2014, 6, 1);

        public Tag[] Tags => new[] { Tag.Software, Tag.Games };

        public void OnGet()
        {
        }
    }
}
