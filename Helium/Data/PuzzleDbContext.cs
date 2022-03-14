using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Helium.Data
{
    public class PuzzleDbContext : DbContext
    {
        public PuzzleDbContext()
        {
        }

        public PuzzleDbContext(DbContextOptions<PuzzleDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Crossword> Crosswords { get; set; } = null!;
        public virtual DbSet<Homophone> Homophones { get; set; } = null!;
        public virtual DbSet<Thesauru> Thesaurus { get; set; } = null!;
        public virtual DbSet<ThesaurusLookup> ThesaurusLookups { get; set; } = null!;
        public virtual DbSet<Word> Words { get; set; } = null!;
    }
}
