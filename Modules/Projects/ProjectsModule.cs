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
            AddStaticRoute("Simulator");
            AddStaticRoute("CodeGell");
            AddStaticRoute("FluxSim");
            AddStaticRoute("SpecialistsCalculator");
            AddStaticRoute("AlgorithmAssistant");
            AddStaticRoute("ElectronicsAndPcb");
            AddStaticRoute("CncMillSoftware");
            AddStaticRoute("Experiments");
            AddStaticRoute("PrintingIntro");
            AddStaticRoute("ChessBoard");
            AddStaticRoute("Models");
            AddStaticRoute("NotableScreenshots");
        }

        /// <summary>
        ///  Adds a route for static data content
        /// </summary>
        /// <param name="routeName">The name of the route to add</param>
        private void AddStaticRoute(string routeName)
        {
            Get[$"/{routeName}"] = parameters => View[routeName];
        }
    }
}