using Helium24.Definitions;
using System;
using System.Collections.Generic;

namespace Helium24.Interfaces
{
    public interface ICameraDataStore
    {
        List<CameraImage> GetImages(DateTime minTime, DateTime maxTime);
        void SaveImage(CameraImage image);
        int DeleteImage(DateTime captureTime);
    }
}
