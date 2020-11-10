using H24.Models;
using Microsoft.Extensions.Logging;

namespace H24
{
    /// <summary>
    /// This class caches the URL of the Database connection string.
    /// </summary>
    public class UrlResolver
    {
        private readonly AppSettings settings;

        public UrlResolver(AppSettings settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Gets the PostgreSQL DB connection string.
        /// </summary>
        public string GetSqlConnectionString()
            => this.settings.DatabaseConnectionFormatString;
    }
}