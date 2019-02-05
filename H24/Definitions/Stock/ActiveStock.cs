using System;

namespace H24.Definitions
{
    /// <summary>
    /// Stocks that are active.
    /// </summary>
    public class ActiveStock
    {
        public string Ticker { get; set; }
        public string Name { get; set; }
        public DateTime DateAcquired { get; set; }
        public float PurchaseAmount { get; set; }
        public float PurchasePrice { get; set; }

        /// <summary>
        /// Splits the given active stock into the amount provided.
        /// </summary>
        internal static ActiveStock Split(ActiveStock activeStock, float amount)
        {
            return new ActiveStock()
            {
                Ticker = activeStock.Ticker,
                Name = activeStock.Name,
                DateAcquired = activeStock.DateAcquired,
                PurchaseAmount = amount,
                PurchasePrice = activeStock.PurchasePrice
            };
        }
    }
}