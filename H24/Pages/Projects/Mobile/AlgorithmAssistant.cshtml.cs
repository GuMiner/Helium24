using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages.Mobile
{
    public class AlgorithmAssistantModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/Mobile/AlgorithmAssistant.png";

        public string Title => "Algorithm Assistant";

        public DateTime Date => new DateTime(2013, 12, 1);

        public Tag[] Tags => new[] { Tag.Software, Tag.Mobile };

        public void OnGet()
        {
        }
    }
}
