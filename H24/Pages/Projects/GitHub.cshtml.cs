using System;
using H24.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages
{
    public class GitHubModel : PageModel, IProjectWithUri
    {
        public GitHubModel(IHostingEnvironment environment)
        {
            ProjectsModel.InitializeProjectsThreadSafe(environment);
        }

        public string ThumbnailUri { get; private set; }

        public string ProjectUri { get; private set; }

        public string Title { get; private set; }

        public DateTime Date { get; private set; }

        public Tag[] Tags { get; private set; }

        public IActionResult OnGet(string gitHubPageName)
        {
            if (ProjectsModel.MarkdownProjects.TryGetValue(gitHubPageName, out IProjectWithUri project))
            {
                this.SetupProject(project);
                return Page();
            }
            else
            {
                return NotFound();
            }
        }

        private void SetupProject(IProjectWithUri projectWithUri)
        {
            this.ThumbnailUri = projectWithUri.ThumbnailUri;
            this.ProjectUri = projectWithUri.ProjectUri;
            this.Title = projectWithUri.Title;
            this.Date = projectWithUri.Date;
            this.Tags = projectWithUri.Tags;
        }
    }
}
