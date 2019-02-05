using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages.Simulation
{
    public class VectorFlowModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/Simulation/VectorFlow.png";

        public string Title => "Vector Flow";

        public DateTime Date => new DateTime(2010, 6, 1);

        public Tag[] Tags => new[] { Tag.Software, Tag.Simulation };

        public void OnGet()
        {
        }
    }
}
