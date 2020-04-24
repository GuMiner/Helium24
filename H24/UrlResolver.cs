using H24.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;

namespace H24
{
    /// <summary>
    /// This class caches the IP address of the remote database, so that DDNS calls don't happen all that regularily.
    /// </summary>
    public class UrlResolver
    {
        private static readonly TimeSpan PersistenceTime = TimeSpan.FromHours(1);
        private readonly ILogger logger;
        private readonly Random random;
        private readonly AppSettings settings;

        private DateTime acquisitionTime;
        private IPAddress address;


        public UrlResolver(ILogger logger, AppSettings settings)
        {
            this.random = new Random();
            this.logger = logger;
            this.settings = settings;

            this.ReloadIPAddresses();
        }

        /// <summary>
        /// Reloads the IP address of the home server using DDNS.
        /// </summary>
        public void ReloadIPAddresses()
        {
            string databaseServer = this.settings.DatabaseServer;

            try
            {
                IPAddress[] addresses = Dns.GetHostAddresses(databaseServer);
                if (addresses != null && addresses.Length > 0)
                {
                    this.address = addresses[0];
                    this.acquisitionTime = DateTime.Now;
                }

                string addressList = string.Join(",", (addresses ?? Array.Empty<IPAddress>()).AsEnumerable());
                this.logger.LogData("DDNSResolution", Guid.Empty.ToString(), new { databaseServer, addressList });
            }
            catch (Exception ex)
            {
                this.logger.LogData("DDNSResolutionFailure", Guid.Empty.ToString(), new { databaseServer, message = ex.Message });
            }
        }

        /// <summary>
        /// Gets the address of the home server.
        /// </summary>
        public string GetServerAddress()
        {
            if (DateTime.Now - acquisitionTime > PersistenceTime)
            {
                ReloadIPAddresses();
            }

            return address.ToString();
        }

        /// <summary>
        /// Gets the PostgreSQL DB connection string.
        /// </summary>
        public string GetSqlConnectionString()
        {
            return string.Format(this.settings.DatabaseConnectionFormatString, this.GetServerAddress());
        }
    }
}