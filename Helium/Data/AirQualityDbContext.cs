using Microsoft.EntityFrameworkCore;

namespace Helium.Data
{
    public class AirQualityDbContext : DbContext
    {
        public AirQualityDbContext()
        {
        }

        public AirQualityDbContext(DbContextOptions<AirQualityDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AirQuality> Measurements { get; set; } = null!;
    }
}
