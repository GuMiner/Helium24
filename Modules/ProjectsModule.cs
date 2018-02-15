using Helium24.Models;
using Nancy;
using System.Linq;

namespace Helium24.Modules
{
    /// <summary>
    /// Module for the personal projects I have completed.
    /// </summary>
    public class ProjectsModule : NancyModule
    {
        public ProjectsModule()
        {
            foreach (Project project in Global.Projects)
            {
                AddStaticRoute(project);
            }
        }
        
        private void AddStaticRoute(Project project)
        {
            if (project.ProjectUri.StartsWith(Project.MarkdownSuffix))
            {
                // Route markdown pages all to the client-side renderer
                string serverUri = project.GetServerSideUri();
                
                Get[serverUri] = parameters => View["MarkdownBasedProject", project.WithoutMarkdownPrefix()];
            }
            else
            {
                // Route realized pages to their actual pages here.
                Get[project.ProjectUri] = 
                    parameters => View[project.ProjectUri.Replace("/Projects/", string.Empty), project];
            }
        }
    }
}