using Helium24.Definitions;
using Nancy;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Helium24.Modules
{
    /// <summary>
    /// Module for retrieving the status of various server resources.
    /// </summary>
    public class StatusModule : NancyModule
    {
        private static int validStatusElements = 0;
        private static int invalidStatusElements = 0;

        public StatusModule()
            : base("/Status")
        {
            Get["/Server"] = parameters =>
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
                    stats[i] = Global.StatsStore.GetSystemStat(intervals[i]);
                    if (stats[i] != null)
                    {
                        ++validStatusElements;
                        Global.Log($"Status/Server retrieved {stats[i].Id} at position {i}.");
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

                        Global.Log($"Status/Server failed to retrieve position {i}.");
                    }
                });

                return this.Response.AsJson(
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
            };

            Get["/Server/Validity"] = parameters => this.Response.AsJson(
                new
                {
                    validElements = validStatusElements,
                    invalidElements = invalidStatusElements
                });

            Get["/PostgreSQL"] = parameters =>
                Global.StatsStore.GetSqlDbStatus();
        }
    }
}