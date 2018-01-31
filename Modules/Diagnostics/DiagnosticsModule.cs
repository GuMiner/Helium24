using Nancy;

namespace Helium24.Modules
{
    public class DiagnosticsModule : NancyModule
    {
        public DiagnosticsModule()
            : base("/Diagnostics")
        {
        }
    }
}
