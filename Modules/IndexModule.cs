using System.Linq;
using Helium24.Models;
using Nancy;

namespace Helium24.Modules
{
    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = parameters => View["Index"];
            Get["/Projects"] = parameters => View["Projects", Global.Projects];
            
            Get["/Diagnostics"] = parameters =>
            {
                DiagnosticsModel model = this.ExtractModelFromHeaders(this.Context.Request.Headers);
                return View["Diagnostics", model];
            };

            Get["/Maps"] = parameters => View["Maps"];
            Get["/Secure"] = parameters => View["Secure"];
            Get["/Error"] = parameters => View["Error"];

            Get["/Reset"] = parameters =>
                this.Authenticate((user) =>
                {
                    Global.ShutdownEvent.Set();
                    return this.Response.AsJson(new { ShutdownMutex = "Set" }, HttpStatusCode.Gone);
                }, true);
        }

        /// <summary>
        /// Extracts the <see cref="DiagnosticsModel"/>  from the headers, returning unknown data on failure.
        /// </summary>
        private DiagnosticsModel ExtractModelFromHeaders(RequestHeaders headers)
        {
            string host = "Unknown";
            string realIp = "Unknown";
            string forwardedFor = "Unknown";

            if (headers.Keys.Contains("X-Real-IP"))
            {
                realIp = string.Join(", ", headers["X-Real-IP"]);
            }

            if (headers.Keys.Contains("Host"))
            {
                host = string.Join(", ", headers["Host"]);
            }

            if (headers.Keys.Contains("X-Forwarded-For"))
            {
                forwardedFor = string.Join(", ", headers["X-Forwarded-For"]);
            }

            DiagnosticsModel model = new DiagnosticsModel()
            {
                ClientAddress = realIp,
                ServerAddress = host + "| " + forwardedFor
            };

            return model;
        }
    }
}