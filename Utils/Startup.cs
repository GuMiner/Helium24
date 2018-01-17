using Owin;

namespace Helium24
{
    /// <summary>
    /// Indicates to OWIN that we're using Nancy.
    /// </summary>
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy(options => options.Bootstrapper = new Helium24NancyBootstrapper());
        }
    }
}
