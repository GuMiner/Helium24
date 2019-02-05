using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace H24.Logging
{
    /// <summary>
    /// Defines a simplified logger that only forwards messages > info, simplifies category names, 
    ///  logs to console or debug (if the debugger is attached), and does no special formatting.
    /// </summary>
    public class SimpleLogger : ILogger
    {
        private readonly bool isDebug;

        private readonly object lockObject;
        private readonly string categoryName;

        public SimpleLogger(string categoryName)
        {
            this.isDebug = Debugger.IsAttached;

            this.categoryName = SimpleLogger.SimplifyCategoryName(categoryName);
            this.lockObject = new object();
        }

        private static string SimplifyCategoryName(string categoryName)
            => categoryName
            .Replace("Microsoft", "MS")
            .Replace(".AspNetCore", string.Empty)
            .Replace(".Hosting", string.Empty)
            .Replace(".Internal", string.Empty)
            .Replace(".DataProtection", string.Empty)
            .Replace(".AuthenticatedEncryption", string.Empty)
            .Replace(".KeyManagement", string.Empty)
            .Replace(".Repositories", string.Empty)
            .Replace("Controller", "C")
            .Replace(".Middleware.RequestResponseLogger", string.Empty);

        /// <summary>
        /// Not supported as there's no easy way to limit based on log level.
        /// </summary>
        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel > LogLevel.Information;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                string message = formatter(state, exception);
                this.Log($"{SimplifiedTime()}: ({this.categoryName}) {message.Replace("\r", string.Empty).Replace("\n", string.Empty)}");
            }
        }

        private string SimplifiedTime() => DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

        private void Log(string message)
        {
            lock (this.lockObject)
            {
                if (this.isDebug)
                {
                    Debug.WriteLine(message);
                }
                else
                {
                    Console.WriteLine(message);
                }
            }
        }
    }
}
