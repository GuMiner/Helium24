using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Helium.Data
{
    [Table("crosswords")]
    public partial class Crossword
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("clue")]
        public string? Clue { get; set; }

        [Column("answer")]
        public string? Answer { get; set; }
    }
}
