using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Helium.Data
{
    [Table("jobs")]
    public partial class Job
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        [Column("prompt")]
        public string Prompt { get; set; }

        [Column("settings")]
        public string Settings { get; set; }

        [Column("imageIds")]
        public string ImageIds { get; set; }

        [Column("isPublic")]
        public long IsPublic { get; set; }
    }
}
