using System;
using System.Collections.Generic;

namespace H24.Definitions.DataStore
{
    public interface ISystemStatsDataStore
    {
        void SaveSystemStat(SystemStat stat);
        string GetSqlDbStatus();
        ICollection<SystemStat> GetSystemStats(DateTime after); // yyyy-MM-dd-HH-mm
    }
}
