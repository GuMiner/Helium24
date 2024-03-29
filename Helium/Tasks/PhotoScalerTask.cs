﻿using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Helium.Tasks
{
    /// <summary>
    /// Scales down photos for the Media page.
    /// </summary>
    public class PhotoScalerTask : IHostedService
    {
        private readonly ILogger<PhotoScalerTask> logger;
        private readonly IHostEnvironment hostEnvironment;
        private Timer? timer;

        public PhotoScalerTask(ILogger<PhotoScalerTask> logger, IHostEnvironment hostEnvironment)
        {
            this.logger = logger;
            this.hostEnvironment = hostEnvironment;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(ScalePhotos, null, TimeSpan.FromSeconds(10), Timeout.InfiniteTimeSpan);
            return Task.CompletedTask;
        }

        private string PhotosPath => Path.Combine(hostEnvironment.ContentRootPath, "wwwroot/img/photos");

        private List<string> FindPhotosToScale()
            => Directory.GetFiles(PhotosPath).ToList();

        private bool ScaledImageExists(string file)
            => File.Exists(GetScaledPath(file));

        private string GetScaledPath(string file)
            => Path.Combine(PhotosPath, "scaled", Path.GetFileName(file));

        private void ScalePhotos(object? unused)
        {
            List<string> photosToScale = FindPhotosToScale();
            foreach (string photoFile in photosToScale.Where(file => !ScaledImageExists(file)))
            {
                logger.LogInformation($"Scaling photo {Path.GetFileName(photoFile)}...");

                // TODO swap away from the MAUI APIs too, because while System.Drawing doesn't support Linux, nor does MAUI!
                // Essentially MAUI isn't really cross platform, as it is only Android/iOS+Windows/macOS. 
                using (Image image = Image.Load(photoFile))
                {
                    image.Mutate(x => x.Resize(new ResizeOptions()
                    {
                        Size = new Size(400, 400),
                        Mode = ResizeMode.Pad,
                        PadColor = Color.White,
                    }));

                    image.SaveAsJpeg(GetScaledPath(photoFile));
                }

                logger.LogInformation($"Scaled photo {Path.GetFileName(photoFile)}");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
