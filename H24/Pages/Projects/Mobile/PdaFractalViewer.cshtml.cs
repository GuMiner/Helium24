using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages.Mobile
{
    public class PdaFractalViewerModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/Mobile/PdaFractalViewer.png";

        public string Title => "iPAQ PDA Fractal Viewer";

        public DateTime Date => new DateTime(2011, 3, 1);

        public Tag[] Tags => new[] { Tag.Software, Tag.Mobile };

        public void OnGet()
        {
        }
    }
}
