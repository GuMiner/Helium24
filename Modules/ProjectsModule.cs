using Helium24.Models;
using Nancy;

namespace Helium24.Modules
{
    /// <summary>
    /// Module for the personal projects I have completed.
    /// </summary>
    public class ProjectsModule : NancyModule
    {
        public ProjectsModule()
            : base("/Projects")
        {
            foreach (Project project in Global.Projects)
            {
                AddStaticRoute(project.ProjectUri.Replace("/Projects", string.Empty), project);
            }
        }
        
        private void AddStaticRoute(string routeName, Project project)
        {
            Get[routeName] = parameters => View[routeName, project];
        }
    }
}