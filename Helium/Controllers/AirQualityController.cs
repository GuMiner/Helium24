using Helium.Data;
using Microsoft.AspNetCore.Mvc;

namespace Helium.Controllers
{
    /// <summary>
    /// Handles saving and returning air quality metrics
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class AirQualityController : ControllerBase
    {
        private readonly AirQualityDbContext db;

        public AirQualityController(AirQualityDbContext db)
        {
            this.db = db;
        }

        [HttpPost]
        public IActionResult Save([FromBody] AirQuality quality, [FromQuery]string sensorId)
        {
            // TODO make this less public
            if (sensorId != "1")
            {
                return this.BadRequest($"Unknown sensor ID {sensorId}");
            }

            if (quality.Timestamp == null)
            {
                // This is expected to normally be null as this simplifies the sensor upload logic
                quality.Timestamp = DateTime.UtcNow.ToString("u");
            }

            this.db.Measurements.Add(quality);
            this.db.SaveChanges();

            return this.Ok(new
            {
                count = this.db.Measurements.Count()
            });
        }

        [HttpGet]
        public IActionResult MostRecent([FromQuery]string sensorId, [FromQuery]int count)
        {
            // TODO make this less public
            if (sensorId != "1")
            {
                return this.BadRequest($"Unknown sensor ID {sensorId}");
            }

            return this.Ok(this.db.Measurements.OrderByDescending(m => m.Timestamp).Take(count));
        }
    }
}