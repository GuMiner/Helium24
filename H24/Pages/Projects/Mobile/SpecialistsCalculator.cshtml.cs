using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages.Mobile
{
    public class SpecialistsCalculatorModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/Mobile/SpecialistsCalculator.png";

        public string Title => "Specialists Calculator";

        public DateTime Date => new DateTime(2014, 8, 1);

        public Tag[] Tags => new[] { Tag.Software, Tag.Mobile };

        public void OnGet()
        {
        }
    }
}
