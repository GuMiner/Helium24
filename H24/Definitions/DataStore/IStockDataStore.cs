namespace H24.Definitions.DataStore
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
