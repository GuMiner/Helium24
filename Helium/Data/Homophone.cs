using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Helium.Data
{
    [Table("homophones")]
    public partial class Homophone
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("homophones")]
        public string? Homophones { get; set; }
    }
}
