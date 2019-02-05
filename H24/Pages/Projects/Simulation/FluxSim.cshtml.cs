using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages.Simulation
{
    public class FluxSimModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/Simulation/FluxSim.png";

        public string Title => "Flux Sim";

        public DateTime Date => new DateTime(2014, 8, 1);

        public Tag[] Tags => new[] { Tag.Software, Tag.Simulation };

        public void OnGet()
        {
        }
    }
}
