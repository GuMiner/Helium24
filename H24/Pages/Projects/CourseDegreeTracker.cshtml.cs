using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages
{
    public class CourseDegreeTrackerModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/CourseDegreeTracker.png";

        public string Title => "Course Degree Tracker";

        public DateTime Date => new DateTime(2012, 12, 1);

        public Tag[] Tags => new[] { Tag.Software };

        public void OnGet()
        {
        }
    }
}
