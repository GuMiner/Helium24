using Nancy;

namespace Helium24.Modules
{
    public class ServerStatusModule : NancyModule
    {
        public ServerStatusModule()
            : base("/ServerStatus")
        {
        }
    }
}
