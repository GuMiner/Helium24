using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Helium.Data
{
    [Table("thesaurus")]
    public partial class Thesauru
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("word")]
        public string? Word { get; set; }

        [Column("synonymIds")]
        public string? SynonymIds { get; set; }
    }
}
