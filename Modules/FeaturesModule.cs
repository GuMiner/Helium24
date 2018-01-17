using Nancy;

namespace Helium24.Modules
{
    /// <summary>
    /// Handles features currently enabled for the given user.
    /// </summary>
    public class FeaturesModule : NancyModule
    {
        public FeaturesModule()
            : base("/Features")
        {
            // Creates a feature for the current user.
            Post["/{feature}"] = parameters =>
            {
                return this.Authenticate((user) =>
                {
                    user.EnabledFeatures.Add(parameters.feature);
                    Global.UserStore.UpdateUserEnabledFeatures(user.UserName, user.EnabledFeatures);
                    return this.Response.AsJson(new { Result = false });
                });
            };

            // Deletes the feature for the current user.
            Delete["/{feature}"] = parameters =>
            {
                return this.Authenticate((user) =>
                {
                    user.EnabledFeatures.Remove(parameters.feature);
                    Global.UserStore.UpdateUserEnabledFeatures(user.UserName, user.EnabledFeatures);
                    return this.Response.AsJson(new { Result = false });
                });
            };
        }
    }
}
