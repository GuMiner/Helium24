using Helium24.Definitions;
using Helium24.Modules;
using System;

namespace Helium24.Tasks
{
    /// <summary>
    /// Saves security images
    /// </summary>
    public class ImageStorageTask
    {
        /// <summary>
        /// Saves images to the DB.
        /// </summary>
        public static void SaveImageTask(object sender, System.Timers.ElapsedEventArgs e)
        {
            Global.Log("Saving camera images...");

            CameraImage cameraOne = GetCameraImage(1, () => CamerasModule.GetCameraOneImage());
            if (cameraOne != null)
            {
                Global.CameraStore.SaveImage(cameraOne);
            }

            CameraImage cameraThree = GetCameraImage(3, () => CamerasModule.GetCameraThreeImage());
            if (cameraThree != null)
            {
                Global.CameraStore.SaveImage(cameraThree);
            }

            Global.Log("Done saving camera images!");
        }

        public static CameraImage GetCameraImage(int cameraId, Func<byte[]> dataAcquisitionAction)
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
                Global.Log($"Error reading from camera {cameraId}: {ex.Message}");
            }

            return null;
        }
    }
}
