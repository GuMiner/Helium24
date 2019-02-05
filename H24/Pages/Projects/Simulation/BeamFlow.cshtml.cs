using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages.Simulation
{
    public class BeamFlowModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/Simulation/BeamFlow.png";

        public string Title => "Beam Flow";

        public DateTime Date => new DateTime(2011, 12, 1);

        public Tag[] Tags => new[] { Tag.Software, Tag.Simulation };

        public void OnGet()
        {
        }
    }
}
