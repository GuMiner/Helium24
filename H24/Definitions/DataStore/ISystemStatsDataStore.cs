namespace H24.Definitions.DataStore
{
    public interface ISystemStatsDataStore
    {
        void SaveSystemStat(SystemStat stat);
        string GetSqlDbStatus();
        SystemStat GetSystemStat(string dateTime); // yyyy-MM-dd-HH-mm
    }
}
