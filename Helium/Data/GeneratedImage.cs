using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Helium.Data
{
    [Table("images")]
    public partial class GeneratedImage
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        [Column("image")]
        public byte[] ImageData { get; set; }
    }
}
