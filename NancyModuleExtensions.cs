using Helium24.Definitions;
using System;
using Nancy;
using Nancy.Security;

namespace Helium24
{
    /// <summary>
    /// Holds route functions to simplify code elsewhere.
    /// </summary>
    public static class NancyModuleExtensions
    {
        /// <summary>
        /// Authenticates as the current user and logs the request message.
        /// </summary>
        public static Response Authenticate(this NancyModule nancyModule, Func<User, Response> action, bool requireAdmin = false)
        {
            nancyModule.RequiresAuthentication();

            User user = nancyModule.Context.CurrentUser as User;
            if (user != null && (!requireAdmin || (requireAdmin && user.IsAdmin)))
            {
                return action(user);
            }

            return nancyModule.Response.AsJson("Unable to verify/determine who you currently are!", HttpStatusCode.Unauthorized);
        }
    }
}