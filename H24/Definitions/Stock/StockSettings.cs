namespace H24.Definitions
{
    /// <summary>
    /// JSON-serializable stock settings
    /// </summary>
    public class StockSettings
    {
        /// <summary>
        /// Holds the Id, which for our purposes is the username who owns the 
        /// </summary>
        public string Id { get; set; }
        public float SellStockFee { get; set; }
        public float TradeCommission { get; set; }
    }
}
