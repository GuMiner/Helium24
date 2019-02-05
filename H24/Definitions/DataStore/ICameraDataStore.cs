using System;
using System.Collections.Generic;

namespace H24.Definitions.DataStore
{
    public interface ICameraDataStore
    {
        List<CameraImage> GetImages(DateTime minTime, DateTime maxTime);
        void SaveImage(CameraImage image);
        int DeleteImage(DateTime captureTime);
    }
}
