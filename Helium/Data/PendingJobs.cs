using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Helium.Data
{
    [Table("pendingJobs")]
    public partial class PendingJob
    {
        [Key]
        [Column("jobId")]
        public string JobId { get; set; }

        [Column("isProcessing")]
        public long IsProcessing { get; set; }
    }
}
