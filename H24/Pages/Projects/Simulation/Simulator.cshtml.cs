using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages.Simulation
{
    public class SimulatorModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/Simulation/Simulator.png";

        public string Title => "Simulator";

        public DateTime Date => new DateTime(2015, 4, 1);

        public Tag[] Tags => new[] { Tag.Software, Tag.Simulation };

        public void OnGet()
        {
        }
    }
}
