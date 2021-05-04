using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using H24.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace H24.Pages
{
    public class ProjectsModel : PageModel
    {
        private static int initializedProjects = 0;
        private static object projectInitLock = new object();
        public static List<IProjectWithUri> Projects { get; private set; }

        /// <summary>
        /// Defines a subset of the projects for URI mapping markdown projects.
        /// </summary>
        public static Dictionary<string, IProjectWithUri> MarkdownProjects { get; private set; }

        public ProjectsModel(IWebHostEnvironment environment)
        {
            ProjectsModel.InitializeProjectsThreadSafe(environment);
        }

        public static void InitializeProjectsThreadSafe(IWebHostEnvironment environment)
        {
            if (ProjectsModel.initializedProjects == 0)
            {
                lock (ProjectsModel.projectInitLock)
                {
                    if (ProjectsModel.initializedProjects == 0)
                    {
                        ProjectsModel.InitializeProjects(environment);
                        ProjectsModel.initializedProjects = 1;
                    }
                }
            }
        }

        private static void InitializeProjects(IWebHostEnvironment environment)
        {
            // Load non-markdown projects
            IEnumerable<IProject> projectTypes = typeof(ProjectsModel).Assembly.GetTypes()
                .Where(t => typeof(IProject).IsAssignableFrom(t) && t.IsClass)
                .Where(t => !t.Name.Contains(nameof(ProjectWithUri)) && !t.Name.Contains(nameof(GitHubModel)))
                .Select(t => Activator.CreateInstance(t) as IProject)
                .ToList();

            ProjectsModel.Projects = projectTypes.Select(project => new ProjectWithUri(project, $"/Projects/{GetProjectUri(project)}") as IProjectWithUri).ToList();

            // Add in markdown projects to the list.
            ProjectsModel.MarkdownProjects = new Dictionary<string, IProjectWithUri>(StringComparer.OrdinalIgnoreCase);

            string projectsPath = Path.Combine(environment.ContentRootPath, "AppContent", "GitHubProjects.json");
            List<ProjectWithUri> markdownProjects = JsonConvert.DeserializeObject<List<ProjectWithUri>>(System.IO.File.ReadAllText(projectsPath));
            foreach (ProjectWithUri project in markdownProjects)
            {
                int mdHeaderLength = "md://".Length;
                int uriDividerLocation = project.ProjectUri.IndexOf('/', mdHeaderLength);
                string pageName = project.ProjectUri.Substring(mdHeaderLength, uriDividerLocation - mdHeaderLength);
                project.ProjectUri = project.ProjectUri.Substring(uriDividerLocation + 1);

                // The Markdown URI is the URI where the page markdown is stored, whereas the projects URI is the inner-page link.
                ProjectsModel.MarkdownProjects.Add(pageName, project);
                ProjectsModel.Projects.Add(new ProjectWithUri(project, $"/Projects/GitHub/{pageName}"));
            }

            ProjectsModel.Projects = ProjectsModel.Projects.OrderByDescending(proj => proj.Date).ToList();
        }

        private static string GetProjectUri(IProject project)
        {
            string uriWithModelAtEnd = project.GetType().FullName.Replace("H24.Pages.", string.Empty).Replace('.', '/');
            return uriWithModelAtEnd.Substring(0, uriWithModelAtEnd.Length - "Model".Length);
        }

        public void OnGet()
        {
        }
    }
}
