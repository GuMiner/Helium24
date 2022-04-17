using Microsoft.EntityFrameworkCore;

namespace Helium.Data
{
    public class SystemDbContext : DbContext
    {
        public SystemDbContext()
        {
        }

        public SystemDbContext(DbContextOptions<SystemDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PerfMetric> PerfMetrics { get; set; } = null!;
    }
}
