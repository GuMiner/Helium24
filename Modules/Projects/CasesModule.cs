using Nancy;

namespace Helium24.Modules
{
    /// <summary>
    /// Module for cases
    /// </summary>
    public class CasesModule : NancyModule
    {
        public CasesModule()
            : base("/Projects/Cases")
        {
            AddStaticRoute("NanoCase");
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