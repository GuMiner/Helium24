using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Bootstrapper;
using Nancy.Security;
using Nancy.TinyIoc;
using System;
using Nancy.Conventions;

namespace Helium24
{
    class Helium24NancyBootstrapper : DefaultNancyBootstrapper
    {
        /// <summary>
        /// Modify our application startup to enable CSRF; adds an error filter.
        /// </summary>
        /// <remarks>
        /// /// To use this functionality, within every 'form' block, after all the inputs add '@Html.AntiForgeryToken()'
        ///  Then, within your POST reception, call 'this.ValidateCsrfToken();, catch the CsrfValidationException,
        ///   and handle the result appropriately.
        /// </remarks>
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            Csrf.Enable(pipelines);
            
            StaticConfiguration.DisableErrorTraces = false;
            pipelines.BeforeRequest += (ctx) =>
            {
                NancyContext context = ctx as NancyContext;
                Global.Log(context.Request.UserHostAddress + ": " + context.Request.Method + " " + context.Request.Url);
                return null; // Don't handle the request, we're just logging it.
            };

            pipelines.OnError += (context, exception) =>
            {
                Console.WriteLine(exception.Message);
                return null;
            };

            FormsAuthentication.Enable(pipelines, new FormsAuthenticationConfiguration()
            {
                RedirectUrl = "/Secure/Login",
                UserMapper = container.Resolve<IUserMapper>()
            });
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
            container.Register<IUserMapper, UserMapper>();
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            // To allow lets-encrypt to renew the certs, call:
            // cd /etc/letsencrypt/certbot
            // service nginx stop
            // service apache2 stop
            // supervisorctl stop helium24
            // ./certbot-auto certonly -d mail.helium24.net -d helium24.net
            // You may need to toggle the website on/off to reduce ram usage.
            this.Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory(".well-known", null));
            base.ConfigureConventions(nancyConventions);
        }

        /// <summary>
        /// Modify our root path to be the application working directory.
        /// </summary>
        protected override IRootPathProvider RootPathProvider
        {
            get { return new Helium24RootPathProvider(); }
        }
    }
}
