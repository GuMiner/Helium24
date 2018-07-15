using Helium24.Definitions;
using Nancy;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Helium24.Modules
{
    public class CamerasModule : NancyModule
    {
        public CamerasModule()
            : base("/Cameras")
        {
            Get["/One"] = parameters =>
            {
                return this.Authenticate((user) =>
                {
                    return this.Response.AsJson(Convert.ToBase64String(GetCameraOneImage()));
                }, requireAdmin: true);
            };

            Get["/Two"] = parameters =>
            {
                return this.Authenticate((user) =>
                {
                    return this.Response.AsJson(Convert.ToBase64String(GetCameraTwoImage()));
                }, requireAdmin: true);
            };

            Get["/Three"] = parameters =>
            {
                return this.Authenticate((user) =>
                {
                    return this.Response.AsJson(Convert.ToBase64String(GetCameraThreeImage()));
                }, requireAdmin: true);
            };

            Get["/OneSensors"] = paramters =>
            {
                return this.Authenticate((user) =>
                {
                    return this.Response.AsJson(this.AuthenticateAndGetAsString(
                        ConfigurationManager.AppSettings["CameraOneCredentials"],
                        Global.UrlResolver.GetCameraOneBaseUri(),
                        Global.UrlResolver.GetCameraStatsAddendum()));
                }, requireAdmin: true);
            };

            Get["/TwoSensors"] = paramters =>
            {
                return this.Authenticate((user) =>
                {
                    return this.Response.AsJson(this.AuthenticateAndGetAsString(
                        ConfigurationManager.AppSettings["CameraTwoCredentials"],
                        Global.UrlResolver.GetCameraTwoBaseUri(),
                        Global.UrlResolver.GetCameraStatsAddendum()));
                }, requireAdmin: true);
            };

            Get["/ThreeSensors"] = paramters =>
            {
                return this.Authenticate((user) =>
                {
                    return this.Response.AsJson(this.AuthenticateAndGetAsString(
                        ConfigurationManager.AppSettings["CameraThreeCredentials"],
                        Global.UrlResolver.GetCameraThreeBaseUri(),
                        Global.UrlResolver.GetCameraStatsAddendum()));
                }, requireAdmin: true);
            };

            Post["/Query"] = parameters =>
            {
                return this.Authenticate((user) =>
                {
                    DateTimeRange range = this.Bind<DateTimeRange>();
                    List<CameraImage> images = Global.CameraStore.GetImages(range.MinDateTime.ToUniversalTime(), range.MaxDateTime.ToUniversalTime());
                    return this.Response.AsJson(images.Select(image =>
                        new
                        {
                            Date = image.CaptureTime,
                            CameraId = image.CameraId,
                            Image = Convert.ToBase64String(image.Image)
                        }));
                }, requireAdmin: true);
            };

            Delete["/Query/Image"] = parameters =>
            {
                return this.Authenticate((user) =>
                {
                    EncapsulatedDateTime dateTime = this.Bind<EncapsulatedDateTime>();
                    int affectedRows = Global.CameraStore.DeleteImage(dateTime.CaptureTime);
                    return this.Response.AsJson(new { RowsAffected = affectedRows });
                }, requireAdmin: true);
            };
        }

        public static byte[] GetCameraOneImage()
        {
            return GetCameraImage(ConfigurationManager.AppSettings["CameraOneCredentials"],
                Global.UrlResolver.GetCameraOneBaseUri() + Global.UrlResolver.GetCameraShotAddendum());
        }

        public static byte[] GetCameraTwoImage()
        {
            return GetCameraImage(ConfigurationManager.AppSettings["CameraTwoCredentials"],
                Global.UrlResolver.GetCameraTwoBaseUri() + Global.UrlResolver.GetCameraShotAddendum());
        }

        public static byte[] GetCameraThreeImage()
        {
            return GetCameraImage(ConfigurationManager.AppSettings["CameraThreeCredentials"],
                Global.UrlResolver.GetCameraThreeBaseUri() + Global.UrlResolver.GetCameraShotAddendum());
        }

        private static byte[] GetCameraImage(string usernamePassword, string url)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", $"{Convert.ToBase64String(Encoding.UTF8.GetBytes($"{usernamePassword}"))}");

                HttpResponseMessage response = client.GetAsync(url).GetAwaiter().GetResult();
                return response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
            }
        }
        
        /// <summary>
        /// Performs basic authentication to the provided url + addendum with a GET operation to that endpoint.
        /// </summary>
        private string AuthenticateAndGetAsString(string usernamePassword, string url, string addendum)
        {
            string responseText = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", $"{Convert.ToBase64String(Encoding.UTF8.GetBytes($"{usernamePassword}"))}");

                HttpResponseMessage response = client.GetAsync(url + addendum).GetAwaiter().GetResult();
                responseText = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            return responseText;
        }
    }
}
