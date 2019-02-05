using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using H24.Definitions;
using H24.Extensions;
using H24.Models;
using H24.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace H24
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly ILogger logger;
        private readonly IResetter resetter;

        /// <summary>
        /// Called periodically to save system stats to Elastic Search.
        /// </summary>
        private static System.Timers.Timer statisticsTimer;

        /// <summary>
        /// Called periodically to save images to the SQL db.
        /// </summary>
        private static System.Timers.Timer imageTimer;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.resetter = new Resetter();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AppSettings settings = new AppSettings();
            services.AddOptions();
            this.configuration.GetSection("Settings").Bind(settings);
            services.Configure<AppSettings>(configuration.GetSection("Settings"));

            // Forms auth
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.LoginPath = new PathString("/Secure/Login"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Website start / stop
            services.AddSingleton(this.resetter);

            // TODO use DI
            Program.UrlResolver = new UrlResolver(this.logger, settings);

            // Startup our auto-tasks
            statisticsTimer = new System.Timers.Timer(1000 * 60 * 10); // Run every 10 minutes.
            statisticsTimer.Elapsed += new SystemStatisticsTask(this.logger).SaveSystemStats;
            statisticsTimer.Start();

            imageTimer = new System.Timers.Timer(1000 * 60 * 20); // Run every 20 minutes (~3 GiB / yr).
            imageTimer.Elapsed += new ImageStorageTask(this.logger, settings).SaveImageTask;
            imageTimer.Start();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            (this.resetter as Resetter).Lifetime = lifetime;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRequestResponseLogger();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
