using System.Diagnostics;

long freeSpace = 0;
long totalSpace = 0;
DriveInfo[] drives = DriveInfo.GetDrives();
foreach (DriveInfo drive in drives.Where(d => d.DriveType == DriveType.Fixed))
{
    freeSpace += drive.AvailableFreeSpace;
    totalSpace += drive.TotalSize;
}

Console.WriteLine((float)freeSpace / (1024 * 1024 * 1024));

var process = Process.GetCurrentProcess();

Console.WriteLine($"Memory Usage: {process.WorkingSet64 / (1024 * 1024):n3} MB. CPU Usage: {process.TotalProcessorTime.TotalMilliseconds}");