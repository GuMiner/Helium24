using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages
{
    public class DiagnosticsModel : PageModel
    {
        public string ServerAddress { get; set; }
        public string ClientAddress { get; set; }

        public void OnGet()
        {
            string host = "Unknown";
            string realIp = "Unknown";
            string forwardedFor = "Unknown";

            IHeaderDictionary headers = this.HttpContext.Request.Headers;
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

            this.ClientAddress = realIp;
            this.ServerAddress = host + "| " + forwardedFor;
        }
    }
}
