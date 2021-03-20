using System.Collections.Generic;

namespace H24.Models
{
    /// <summary>
    /// Indirectly reads appsettings.json.
    /// </summary>
    public class AppSettings
    {
        public string DatabaseConnectionFormatString { get; set; }
        
        // These need periodic updating to stay up-to-date!
        public Dictionary<string, float> AssetAllocations { get; set; } // How much of each ETF I have
        public Dictionary<string, float> AssetCostBasis { get; set; } // How much each ETF originally cost

        public Dictionary<string, List<string>> AssetsInCategories { get; set; } // How to categorize each ETF
    }
}
