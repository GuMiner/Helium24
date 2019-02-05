using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace H24
{
    public static class ILoggerExtensions
    {
        public static void LogData(this ILogger logger, string name, string traceId, object @object = null)
            => logger.Log(LogLevel.Warning, null, $"{name} | {traceId} | {JsonConvert.SerializeObject(@object)}");
    }
}
