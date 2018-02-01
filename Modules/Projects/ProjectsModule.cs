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
            AddStaticRoute("FluxSim");
            AddStaticRoute("GeigerCounter");
            AddStaticRoute("Simulator");
            AddStaticRoute("CodeGell");
            AddStaticRoute("CncMillSoftware");
            AddStaticRoute("ChessBoard");
            AddStaticRoute("QuantumComputing");

            AddStaticRoute("ElectronicsAndPcb");
            AddStaticRoute("Experiments");
            AddStaticRoute("PrintingIntro");
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