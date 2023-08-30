using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Helium.Data
{
    [Table("measurements")]
    public partial class AirQuality
    {
        [Key]
        [Column("timestamp")]
        public string? Timestamp { get; set; }

        [Column("pressureHPa")]
        public double PressureHPa { get; set; }

        [Column("altitudeFt")]
        public double AltitudeFt { get; set; }

        [Column("temperatureC")]
        public double TemperatureC { get; set; }

        [Column("co2Ppm")]
        public double CO2Ppm { get; set; }
    }
}
