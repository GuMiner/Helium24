using H24.Definitions;
using H24.Time;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace H24.Tasks
{
    /// <summary>
    /// Saves system stats.
    /// </summary>
    public class SystemStatisticsTask
    {
        private readonly ILogger logger;

        public SystemStatisticsTask(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Saves system stats to the DB.
        /// </summary>
        public void SaveSystemStats(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.logger.LogData("SaveSystemStatsStart", Guid.Empty.ToString());
            SystemStat stat = new SystemStat();
            stat.Id = TimeUtils.GetDateTimeToNearestTenMinutes();

            long freeSpace = 0;
            long totalSpace = 0;
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives.Where(d => d.DriveType == DriveType.Fixed))
            {
                freeSpace += drive.AvailableFreeSpace;
                totalSpace += drive.TotalSize;
            }

            stat.DiskFreePercentage = (float)freeSpace / (float)totalSpace;

            using (Process currentProcess = Process.GetCurrentProcess())
            {
                stat.UserProcessorPercent = (float)(currentProcess.UserProcessorTime.TotalMilliseconds / currentProcess.TotalProcessorTime.TotalMilliseconds);
            }

            Program.StatsStore.SaveSystemStat(stat);
            this.logger.LogData("SaveSystemStatsStop", Guid.Empty.ToString());
        }
    }
}
