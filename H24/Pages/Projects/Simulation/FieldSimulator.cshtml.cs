using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages.Simulation
{
    public class FieldSimulatorModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/Simulation/FieldSimulator.png";

        public string Title => "Field Simulator";

        public DateTime Date => new DateTime(2009, 1, 1);

        public Tag[] Tags => new[] { Tag.Software, Tag.Simulation };

        public void OnGet()
        {
        }
    }
}
