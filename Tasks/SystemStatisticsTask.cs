using Helium24.Definitions;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Helium24.Tasks
{
    /// <summary>
    /// Saves system stats.
    /// </summary>
    public class SystemStatisticsTask
    {
        /// <summary>
        /// Saves system stats to the DB.
        /// </summary>
        public static void SaveSystemStats(object sender, System.Timers.ElapsedEventArgs e)
        {
            Global.Log("Saving system stats...");
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

            Global.StatsStore.SaveSystemStat(stat);
            Global.Log("Done saving system stats!");
        }
    }
}
