namespace H24.Models
{
    /// <summary>
    /// Holds information relevant for the home page.
    /// </summary>
    public class AppSettings
    {
        public string BlobHash { get; set; }

        public string CameraOneFormatString { get; set; }
        public string CameraOneCredentials { get; set; }

        public string CameraTwoFormatString { get; set; }
        public string CameraTwoCredentials { get; set; }

        public string CameraThreeFormatString { get; set; }
        public string CameraThreeCredentials { get; set; }

        public string DatabaseServer { get; set; }
        public string DatabaseConnectionFormatString { get; set; }

        public string QuoteUri { get; set; }
    }
}
