using H24.Logging;
using Microsoft.Extensions.Logging;

namespace H24.Extensions
{
    public static class ILoggingBuilderExtensions
    {
        public static void AddJsonLogger(this ILoggingBuilder loggingBuilder)
            => loggingBuilder.AddProvider(new SimpleLoggerProvider());
    }
}
