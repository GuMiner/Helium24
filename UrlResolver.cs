using System;
using System.Configuration;
using System.Linq;
using System.Net;

namespace Helium24
{
    /// <summary>
    /// This class caches the IP address of the remote database, so that DDNS calls don't happen all that regularily.
    /// </summary>
    public class UrlResolver
    {
        private static readonly TimeSpan PersistenceTime = TimeSpan.FromHours(1);

        private DateTime acquisitionTime;
        private IPAddress address;

        private Random random;

        public UrlResolver()
        {
            ReloadIPAddresses();
            random = new Random();
        }

        /// <summary>
        /// Reloads the IP address of the home server using DDNS.
        /// </summary>
        public void ReloadIPAddresses()
        {
            try
            {
                string databaseServer = ConfigurationManager.AppSettings["DatabaseServer"];
                IPAddress[] addresses = Dns.GetHostAddresses(databaseServer);
                if (addresses != null && addresses.Length > 0)
                {
                    address = addresses[0];
                    acquisitionTime = DateTime.Now;

                    string addressList = string.Join(",", addresses.AsEnumerable());
                    Global.Log($"Performed DDNS resolution from '{databaseServer}' to '{addressList}'.");
                }
                else
                {
                    Global.Log("Couldn't load any addresses from our DDNS provider!");
                }
            }
            catch (Exception ex)
            {
                Global.Log($"Error during DDNS resolution: {ex.Message}.");
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
        /// Returns the base URI to use to communicate to camera one with.
        /// </summary>
        public string GetCameraOneBaseUri()
        {
            return string.Format(ConfigurationManager.AppSettings["CameraOneFormatString"], GetServerAddress());
        }

        /// <summary>
        /// Returns the base URI to use to communicate to camera two with.
        /// </summary>
        public string GetCameraTwoBaseUri()
        {
            return string.Format(ConfigurationManager.AppSettings["CameraTwoFormatString"], GetServerAddress());
        }

        /// <summary>
        /// Returns the base URI to use to communicate to camera three with.
        /// </summary>
        public string GetCameraThreeBaseUri()
        {
            return string.Format(ConfigurationManager.AppSettings["CameraThreeFormatString"], GetServerAddress());
        }

        public string GetCameraShotAddendum()
        {
            return $"/shot.jpg?rnd={random.Next(1, 1000000)}";
        }

        public string GetCameraStatsAddendum()
        {
            return "/sensors.json?sense=battery_temp,battery_level,battery_voltage";
        }

        /// <summary>
        /// Gets the PostgreSQL DB connection string.
        /// </summary>
        public string GetSqlConnectionString()
        {
            return string.Format(ConfigurationManager.AppSettings["DatabaseConnectionFormatString"], GetServerAddress());
        }
    }
}