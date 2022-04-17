using Helium.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Helium.Tasks
{
    public class SystemStatusTask : IHostedService
    {
        private readonly IDbContextFactory<SystemDbContext> dbFactory;
        private Timer? timer;
        private Stopwatch executionTimer;
        private double lastCpuUsage;

        public SystemStatusTask(IDbContextFactory<SystemDbContext> dbFactory)
        {
            this.dbFactory = dbFactory;

            executionTimer = Stopwatch.StartNew();
            lastCpuUsage = Process.GetCurrentProcess().TotalProcessorTime.TotalMilliseconds;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(SaveStats, null, TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(10));
            return Task.CompletedTask;
        }

        private void SaveStats(object? unused)
        {
            // Get disk, memory, and cpu usage
            DriveInfo[] drives = DriveInfo.GetDrives();
            long freeSpaceInGb = drives.Where(d => d.DriveType == DriveType.Fixed).Sum(d => d.AvailableFreeSpace)
                / (1024 * 1024 * 1024);

            var process = Process.GetCurrentProcess();

            long memoryUsageMb = process.WorkingSet64 / (1024 * 1024);

            var currentCpuUsage = process.TotalProcessorTime.TotalMilliseconds;
            var timeDelta = executionTimer.ElapsedMilliseconds;
            double cpuUsage = ComputePercentCpuUsage(timeDelta, currentCpuUsage);

            using (var db = dbFactory.CreateDbContext())
            {
                db.PerfMetrics.Add(new PerfMetric()
                {
                    Timestamp = TimeRounding.GetDateTimeToNearestTenMinutes(),
                    DiskUsageGb = freeSpaceInGb,
                    MemoryUsageMb = memoryUsageMb,
                    CpuPercentUsage = cpuUsage
                });

                db.SaveChanges();
            }
        }

        private double ComputePercentCpuUsage(long elapsedMilliseconds, double currentCpuUsage)
        {
            double totalCpuUsage = currentCpuUsage - lastCpuUsage;
            lastCpuUsage = currentCpuUsage;
            return totalCpuUsage / elapsedMilliseconds;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
