using Helium24.Definitions;

namespace Helium24.Interfaces
{
    public interface IStockDataStore
    {
        ActivePositions GetActivePositions(string userName);
        void SaveActivePositions(ActivePositions activePositions);
        void UpdateActivePositions(ActivePositions activePositions);

        StockSettings GetStockSettings(string userName);
        void SaveStockSettings(StockSettings settings);
        void UpdateStockSettings(StockSettings settings);
    }
}
