using Nancy;

namespace Helium24.Modules
{
    /// <summary>
    /// Module for the personal projects I have completed.
    /// </summary>
    public class MobileModule : NancyModule
    {
        public MobileModule()
            : base("/Projects/Mobile")
        {
            AddStaticRoute("AlgorithmAssistant");
            AddStaticRoute("SpecialistsCalculator");
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