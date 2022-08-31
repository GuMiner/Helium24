using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Helium.Data
{
    [Table("users")]
    public partial class User
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("accessKey")]
        public string AccessKey { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
}
