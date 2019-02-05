using H24.Definitions;
using H24.Models;
using H24.Modules;
using Microsoft.Extensions.Logging;
using System;

namespace H24.Tasks
{
    /// <summary>
    /// Saves security images
    /// </summary>
    public class ImageStorageTask
    {
        private readonly ILogger logger;
        private readonly AppSettings settings;

        public ImageStorageTask(ILogger logger, AppSettings settings)
        {
            this.logger = logger;
            this.settings = settings;
        }

        /// <summary>
        /// Saves images to the DB.
        /// </summary>
        public void SaveImageTask(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.logger.LogData("StoreImagesStart", Guid.Empty.ToString());
            int imagesSaved = 0;

            CameraImage cameraOne = this.GetCameraImage(1, () => CamerasController.GetCameraOneImage(this.settings.CameraOneCredentials));
            if (cameraOne != null)
            {
                Program.CameraStore.SaveImage(cameraOne);
                imagesSaved |= 0x001;
            }

            CameraImage cameraTwo = this.GetCameraImage(2, () => CamerasController.GetCameraTwoImage(this.settings.CameraTwoCredentials));
            if (cameraTwo != null)
            {
                Program.CameraStore.SaveImage(cameraTwo);
                imagesSaved |= 0x010;
            }

            CameraImage cameraThree = this.GetCameraImage(3, () => CamerasController.GetCameraThreeImage(this.settings.CameraThreeCredentials));
            if (cameraThree != null)
            {
                Program.CameraStore.SaveImage(cameraThree);
                imagesSaved |= 0x100;
            }

            this.logger.LogData("StoreImagesStop", Guid.Empty.ToString(), new { imagesSaved });
        }

        public CameraImage GetCameraImage(int cameraId, Func<byte[]> dataAcquisitionAction)
        {
            try
            {
                return new CameraImage()
                {
                    CaptureTime = DateTime.UtcNow,
                    CameraId = cameraId,
                    Image = dataAcquisitionAction()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogData("GetCameraImageError", Guid.Empty.ToString(), new { cameraId, message = ex.Message });
            }

            return null;
        }
    }
}
