using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace H24.Middleware
{
    public class RequestResponseLogger
    {
        private readonly RequestDelegate nextDelegate;
        private readonly ILogger logger;

        public RequestResponseLogger(RequestDelegate nextDelegate, ILoggerFactory loggerFactory)
        {
            this.nextDelegate = nextDelegate;
            this.logger = loggerFactory.CreateLogger<RequestResponseLogger>();
        }

        public async Task Invoke(HttpContext context)
        {
            logger.LogData("Request", context.TraceIdentifier, new { context.Request.Host.Host, context.Request.Host.Port, context.Request.Path, context.Request.Method, context.Request.Query });
            await this.nextDelegate(context);
            logger.LogData("Response", context.TraceIdentifier, new { context.Response.StatusCode });
        }
    }
}
