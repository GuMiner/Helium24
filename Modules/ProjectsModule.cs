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
                AddStaticRoute(project.ProjectUri, project);
            }
        }
        
        private void AddStaticRoute(string routeName, Project project)
        {
            Get[routeName] = parameters => View[routeName.Replace("/Projects/", string.Empty), project];
        }
    }
}