using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages
{
    public class QuantumComputingModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/QuantumComputing.png";

        public string Title => "Quantum Computing";

        public DateTime Date => new DateTime(2018, 2, 1);

        public Tag[] Tags => new[] { Tag.Simulation, Tag.Software };

        public void OnGet()
        {
        }
    }
}
