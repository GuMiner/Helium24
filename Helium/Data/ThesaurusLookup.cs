using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Helium.Data
{
    [Table("thesaurus_lookup")]
    public partial class ThesaurusLookup
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("synonymList")]
        public string? SynonymList { get; set; }
    }
}
