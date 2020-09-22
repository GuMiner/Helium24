using H24.Definitions;
using H24.Time;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace H24.Modules
{
    /// <summary>
    /// Module for retrieving the status of various server resources.
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class StatusController : ControllerBase
    {
        private static int validStatusElements = 0;
        private static int invalidStatusElements = 0;

        private readonly ILogger logger;

        public StatusController(ILogger<StatusController> logger)
        {
            this.logger = logger;
        }

        public IActionResult Server()
        {
            ICollection<SystemStat> stats = Program.StatsStore.GetSystemStats(
                DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)));
            this.logger.LogInformation($"Retrieved {stats.Count} stats.");

            return this.Ok(
                new
                {
                    labels = stats.Select(stat => stat.Id).ToArray(),
                    series = new[] {
                        new {
                            name = "CPU Utilization",
                            data = stats.Select(stat => stat.UserProcessorPercent).ToArray()
                        },
                        new
                        {
                            name = "Disk Usage %",
                            data = stats.Select(stat => stat.DiskFreePercentage).ToArray()
                        }
                    }
                });
        }

        public IActionResult ServerValidity() => this.Ok(
            new
            {
                validElements = validStatusElements,
                invalidElements = invalidStatusElements
            });

        public ActionResult<string> PostgreSQL() => 
            this.Ok(Program.StatsStore.GetSqlDbStatus());
    }
}