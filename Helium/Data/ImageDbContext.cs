using Microsoft.EntityFrameworkCore;

namespace Helium.Data
{
    public class ImageDbContext : DbContext
    {
        public ImageDbContext()
        {
        }

        public ImageDbContext(DbContextOptions<ImageDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<GeneratedImage> Images { get; set; } = null!;
        public virtual DbSet<Job> Jobs { get; set; } = null!;
        public virtual DbSet<UserJob> UserJobs { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
    }
}
