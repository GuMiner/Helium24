using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages.Mobile
{
    public class PdaLissajousCurvesModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/Mobile/PdaLissajousCurves.png";

        public string Title => "iPAQ PDA Lissajous Curves";

        public DateTime Date => new DateTime(2010, 8, 1);

        public Tag[] Tags => new[] { Tag.Software, Tag.Mobile };

        public void OnGet()
        {
        }
    }
}
