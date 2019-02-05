using H24.Definitions;
using H24.Time;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
            string[] intervals = TimeUtils.GetLastDayInTenMinutes();
            SystemStat[] stats = new SystemStat[intervals.Length];

            validStatusElements = 0;
            invalidStatusElements = 0;

            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = 10;
            Random rand = new Random();
            ParallelLoopResult parallelismResult = Parallel.For(0, intervals.Length, parallelOptions, (i) =>
            {
                stats[i] = Program.StatsStore.GetSystemStat(intervals[i]);
                if (stats[i] != null)
                {
                    ++validStatusElements;
                    this.logger.LogData("RetrivedStat", this.HttpContext.TraceIdentifier, new { Id = stats[i].Id, Position = i });
                }
                else
                {
                    ++invalidStatusElements;
                    stats[i] = new SystemStat()
                    {
                        Id = intervals[i],
                        DiskFreePercentage = (float)(10.0 + rand.NextDouble() * 1.0) / 100.0f,
                        UserProcessorPercent = (float)(20.0 + rand.NextDouble() * 10.0) / 100.0f
                    };

                    this.logger.LogData("RetrivedStatError", this.HttpContext.TraceIdentifier, new { Position = i });
                }
            });

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