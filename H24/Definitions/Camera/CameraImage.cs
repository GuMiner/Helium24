using System;

namespace H24.Definitions
{
    public class CameraImage
    {
        public DateTime CaptureTime { get; set; }
        public int CameraId { get; set; }
        public byte[] Image { get; set; }
    }
}
