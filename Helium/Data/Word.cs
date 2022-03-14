using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Helium.Data
{
    [Table("words")]
    public partial class Word
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("word")]
        public string WordValue { get; set; } = null!;
    }
}
