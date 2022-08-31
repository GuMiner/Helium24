using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Helium.Data
{
    [Table("userJobs")]
    public partial class UserJob
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        [Column("userId")]
        public long UserId { get; set; }

        [Column("jobId")]
        public string JobId { get; set; }

        [Column("creationDate")]
        public string CreationDate { get; set; }
    }
}
