using Helium24.Definitions;

namespace Helium24.Interfaces
{
    public interface ISystemStatsDataStore
    {
        void SaveSystemStat(SystemStat stat);
        string GetSqlDbStatus();
        SystemStat GetSystemStat(string dateTime); // yyyy-MM-dd-HH-mm
    }
}
