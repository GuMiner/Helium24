using Helium24.Definitions;
using Helium24.Interfaces;
using Helium24.Models;
using Helium24.Tasks;
using Microsoft.Owin.Hosting;
using Nancy.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Helium24
{
    /// <summary>
    /// Starts OWIN (which starts Nancy) from the command line.
    /// </summary>
    public class Global
    {
        public static ICameraDataStore CameraStore { get; private set; }
        public static INotesDataStore NotesStore { get; private set; }
        public static ISystemStatsDataStore StatsStore { get; private set; }
        public static IUserDataStore UserStore { get; private set; }
        public static IStockDataStore StockStore { get; private set; }
        public static IDocumentationDataStore DocsStore { get; private set; }
        public static IMapsDataStore MapsStore { get; private set; }

        // TODO: Make this an IoC instead of a static data type
        public static List<Project> Projects { get; private set; }

        /// <summary>
        /// Action to perform when logging.
        /// </summary>
        public static Action<string> Log => (input) => Console.WriteLine(input);

        /// <summary>
        /// Event we wait on to not shutdown the system.
        /// </summary>
        public static ManualResetEvent ShutdownEvent { get; private set; }

        /// <summary>
        /// Used to resolve URLs to avoid pinging the DDNS too much.
        /// </summary>
        public static UrlResolver UrlResolver { get; private set; }

        /// <summary>
        /// Called periodically to save system stats to Elastic Search.
        /// </summary>
        private static System.Timers.Timer statisticsTimer;

        /// <summary>
        /// Called periodically to save images to the SQL db.
        /// </summary>
        private static System.Timers.Timer imageTimer;

        public static void Main(string[] args)
        {
            // Setup the data stores
            // TODO: Use IoC built into our web framework.
            SqlDataStore dataStore = new SqlDataStore();
            Global.CameraStore = dataStore;
            Global.NotesStore = dataStore;
            Global.StatsStore = dataStore;
            Global.UserStore = dataStore;
            Global.StockStore = dataStore;
            Global.DocsStore = dataStore;
            Global.MapsStore = dataStore;

            List<SerializedProject> serializedProjects =
                JsonConvert.DeserializeObject<List<SerializedProject>>(File.ReadAllText("./Content/Projects.json"));
            Global.Projects = serializedProjects.Select(item => (Project)item).ToList();
            
            // Turn off certificate validation, because it doesn't work with self-signed stuffs.
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                {
                    // TODO make this check to validate it is the expected cert.
                    return true;
                };
            
            // To get port 80, you'll need route port 8080 to port 80 through this: "sudo /sbin/iptables -t nat -A PREROUTING -i eth+ -p tcp --dport 80 -j REDIRECT --to-port 8080"
            if (args.Length != 1)
            {
                Log("Expecting the startup URL as the singleton argument!");
            }
            else
            {
                Log("Helium24 Website Startup.");

                Global.ShutdownEvent = new ManualResetEvent(false);
                Global.UrlResolver = new UrlResolver();

                // Allow large binary content to be passed as JSON.
                JsonSettings.MaxJsonLength = Int32.MaxValue;

                // Startup our auto-tasks
                statisticsTimer = new System.Timers.Timer(1000 * 60 * 10); // Run every 10 minutes.
                statisticsTimer.Elapsed += SystemStatisticsTask.SaveSystemStats;
                statisticsTimer.Start();

                imageTimer = new System.Timers.Timer(1000 * 60 * 20); // Run every 20 minutes (~3 GiB / yr).
                imageTimer.Elapsed += ImageStorageTask.SaveImageTask;
                imageTimer.Start();

                using (WebApp.Start<Startup>(args[0]))
                {
                    Global.ShutdownEvent.WaitOne();
                    Global.Log("Helium24 Website Shutdown.");
                }
            }
        }
    }
}
