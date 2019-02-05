using Microsoft.Extensions.Logging;

namespace H24.Logging
{
    /// <summary>
    /// Defines a logging provider for JSON logging
    /// </summary>
    internal class SimpleLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
            => new SimpleLogger(categoryName);

        public void Dispose() { }
    }
}
