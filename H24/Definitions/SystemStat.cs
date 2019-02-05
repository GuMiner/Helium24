namespace H24.Definitions
{
    /// <summary>
    /// JSON-serializable system status.
    /// </summary>
    public class SystemStat
    {
        /// <summary>
        /// Holds the Id, which for our purposes is yyyy-MM-dd-HH-mm
        /// </summary>
        public string Id { get; set; }
        public float UserProcessorPercent { get; set; }
        public float DiskFreePercentage { get; set; }
    }
}
