using H24.Definitions;
using H24.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace H24.Modules
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class CamerasController : ControllerBase
    {
        private readonly AppSettings settings;

        public CamerasController(IOptions<AppSettings> settings)
        {
            this.settings = settings.Value;
        }

        [HttpGet]
        public ActionResult<string> One()
        {
            return Convert.ToBase64String(GetCameraOneImage(this.settings.CameraOneCredentials));
        }

        [HttpGet]
        public ActionResult<string> Two()
        {
            return Convert.ToBase64String(GetCameraTwoImage(this.settings.CameraTwoCredentials));
        }

        [HttpGet]
        public ActionResult<string> Three()
        {
            return Convert.ToBase64String(GetCameraThreeImage(this.settings.CameraThreeCredentials));
        }

        [HttpGet]
        public ActionResult<string> OneSensors()
        {
            return this.AuthenticateAndGetAsString(
                this.settings.CameraOneCredentials,
                Program.UrlResolver.GetCameraOneBaseUri(),
                Program.UrlResolver.GetCameraStatsAddendum());
        }

        [HttpGet]
        public ActionResult<string> TwoSensors()
        {
            return this.AuthenticateAndGetAsString(
                this.settings.CameraTwoCredentials,
                Program.UrlResolver.GetCameraTwoBaseUri(),
                Program.UrlResolver.GetCameraStatsAddendum());
        }

        [HttpGet]
        public ActionResult<string> ThreeSensors()
        {
            return this.AuthenticateAndGetAsString(
                this.settings.CameraThreeCredentials,
                Program.UrlResolver.GetCameraThreeBaseUri(),
                Program.UrlResolver.GetCameraStatsAddendum());
        }

        [HttpGet]
        public IActionResult Query([FromBody]DateTimeRange range)
        {
            List<CameraImage> images = Program.CameraStore.GetImages(range.MinDateTime.ToUniversalTime(), range.MaxDateTime.ToUniversalTime());
            return this.Ok(images.Select(image =>
                new
                {
                    Date = image.CaptureTime,
                    CameraId = image.CameraId,
                    Image = Convert.ToBase64String(image.Image)
                }));
        }

        [HttpPost]
        [ProducesResponseType(400)]
        public IActionResult QueryImage([FromBody]EncapsulatedDateTime dateTime)
        {
                int affectedRows = Program.CameraStore.DeleteImage(dateTime.CaptureTime);
                return this.Ok(new { RowsAffected = affectedRows });
        }

        public static byte[] GetCameraOneImage(string credentials)
        {
            return GetCameraImage(credentials,
                Program.UrlResolver.GetCameraOneBaseUri() + Program.UrlResolver.GetCameraShotAddendum());
        }

        public static byte[] GetCameraTwoImage(string credentials)
        {
            return GetCameraImage(credentials,
                Program.UrlResolver.GetCameraTwoBaseUri() + Program.UrlResolver.GetCameraShotAddendum());
        }

        public static byte[] GetCameraThreeImage(string credentials)
        {
            return GetCameraImage(credentials,
                Program.UrlResolver.GetCameraThreeBaseUri() + Program.UrlResolver.GetCameraShotAddendum());
        }

        private static byte[] GetCameraImage(string usernamePassword, string url)
        {
            using (HttpClient client = CamerasController.GetSslPermissiveClient())
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
            using (HttpClient client = CamerasController.GetSslPermissiveClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", $"{Convert.ToBase64String(Encoding.UTF8.GetBytes($"{usernamePassword}"))}");

                HttpResponseMessage response = client.GetAsync(url + addendum).GetAwaiter().GetResult();
                responseText = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            return responseText;
        }

        private static HttpClient GetSslPermissiveClient()
        {
            return new HttpClient(
                new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (message, chain, cert, errors) =>
                    {
                        // TODO: Restrict based on cert thumbprints based on location.
                        return true;
                    }
                },
                true);
        }
    }
}
