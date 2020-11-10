using H24.Definitions.DataStore;
using H24.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace H24
{
    public class Program
    {
        // TODO: Make this an IoC instead of a static data type
        public static INotesDataStore NotesStore { get; private set; }
        public static ISystemStatsDataStore StatsStore { get; private set; }
        public static IUserDataStore UserStore { get; private set; }

        /// <summary>
        /// Used to resolve URLs to avoid pinging the DDNS too much.
        /// </summary>
        public static UrlResolver UrlResolver { get; set; }

        public static void Main(string[] args)
        {
            IWebHost webHost = CreateWebHostBuilder(args).Build();
            ILogger logger = webHost.Services.GetRequiredService<ILogger<Startup>>();

            // Setup the data stores
            // TODO: Use IoC built into our web framework.
            SqlDataStore dataStore = new SqlDataStore(logger);
            Program.NotesStore = dataStore;
            Program.StatsStore = dataStore;
            Program.UserStore = dataStore;

            logger.LogData("WebsiteStart", Guid.Empty.ToString(), new { SiteName = "Helium24" });
            webHost.Run();
            logger.LogData("WebsiteStop", Guid.Empty.ToString());
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((context, loggingBuilder) =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddJsonLogger();
                    loggingBuilder.AddFilter("Microsoft", LogLevel.Warning);
                })
                .UseStartup<Startup>();
    }
}
