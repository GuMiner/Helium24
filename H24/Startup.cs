using H24.Definitions;
using H24.Extensions;
using H24.Models;
using H24.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.resetter = new Resetter();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient(); // IHttpClientFactory

            AppSettings settings = new AppSettings();
            services.AddOptions();
            this.configuration.GetSection("Settings").Bind(settings);
            services.Configure<AppSettings>(configuration.GetSection("Settings"));

            // Forms auth
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.LoginPath = new PathString("/Secure/Login"));
            services.AddMvc(mvcOptions => { mvcOptions.EnableEndpointRouting = false; }).SetCompatibilityVersion(CompatibilityVersion.Latest);

            // Website start / stop
            services.AddSingleton(this.resetter);

            // TODO use DI
            Program.UrlResolver = new UrlResolver(settings);

            // Startup our auto-tasks
            statisticsTimer = new System.Timers.Timer(1000 * 60 * 10); // Run every 10 minutes.
            statisticsTimer.Elapsed += new SystemStatisticsTask(this.logger).SaveSystemStats;
            statisticsTimer.Start();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
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
