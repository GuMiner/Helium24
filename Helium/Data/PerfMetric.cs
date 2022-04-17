using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Helium.Data
{
    [Table("perfMetrics")]
    public partial class PerfMetric
    {
        [Key]
        [Column("timestamp")]
        public string? Timestamp { get; set; }

        [Column("diskUsageGb")]
        public long DiskUsageGb { get; set; }

        [Column("memoryUsageMb")]
        public long MemoryUsageMb { get; set; }

        [Column("cpuPercentUsage")]
        public double CpuPercentUsage { get; set; }
    }
}
