using Helium24.Definitions;
using Nancy;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Helium24.Modules
{
    /// <summary>
    /// Handles Stock API operations to track stocks for general alerting.
    /// </summary>
    public class StockModule : NancyModule
    {
        public StockModule()
            : base("/Stock")
        {
            // Performs a stock purchase
            Post["/Purchase"] = parameters =>
            {
                return this.Authenticate((user) =>
                {
                    ActiveStock activeStock = this.Bind<ActiveStock>();
                    if (activeStock != null)
                    {
                        if (activeStock.PurchaseAmount < 0 || activeStock.PurchasePrice < 0)
                        {
                            return this.Response.AsJson("The purchase amount and price must both be > 0.", HttpStatusCode.BadRequest);
                        }
                        ActivePositions activePositions = GetOrCreateActivePositions(user.UserName);

                        activePositions.Positions.Add(activeStock);
                        activePositions.Positions.Sort((first, second) => first.DateAcquired.CompareTo(second.DateAcquired));
                        Global.StockStore.UpdateActivePositions(activePositions);
                        return this.Response.AsText(string.Empty);
                    }

                    return this.Response.AsJson("Unable to process request!", HttpStatusCode.BadRequest);
                });
            };

            // Sells a stock position, either partially or completely.
            Post["/Sale"] = parameters =>
            {
                return this.Authenticate((user) =>
                {
                    StockSale selection = this.Bind<StockSale>();
                    ActivePositions activePositions = GetOrCreateActivePositions(user.UserName);

                    if (selection.Amount <= 0)
                    {
                        return this.Response.AsJson("The amount to sell must be greater than zero!");
                    }

                    if (selection.SelectedIndex >= 0 && selection.SelectedIndex < activePositions.Positions.Count)
                    {
                        ActiveStock activeStock = activePositions.Positions[selection.SelectedIndex];

                        if (Math.Abs(activeStock.PurchaseAmount - selection.Amount) < 0.00001f)
                        {
                            activePositions.Positions.RemoveAt(selection.SelectedIndex);
                            Global.StockStore.UpdateActivePositions(activePositions);
                            return this.Response.AsText(string.Empty);
                        }
                        else
                        {
                            if (selection.Amount > activeStock.PurchaseAmount)
                            {
                                return this.Response.AsJson("The amount to sell must not be more than the purchased amount!", HttpStatusCode.BadRequest);
                            }

                            activePositions.Positions.RemoveAt(selection.SelectedIndex);
                            activePositions.Positions.Add(ActiveStock.Split(activeStock, activeStock.PurchaseAmount - selection.Amount));
                            activePositions.Positions.Sort((first, second) => first.DateAcquired.CompareTo(second.DateAcquired));
                            Global.StockStore.UpdateActivePositions(activePositions);
                            return this.Response.AsText(string.Empty);
                        }
                    }

                    return this.Response.AsJson("Unable to process request!", HttpStatusCode.BadRequest);
                });
            };

            // Gets the active positions for the current user.
            Get["/ActivePositions"] = parameters =>
            {
                return this.Authenticate((user) =>
                    this.Response.AsJson(GetOrCreateActivePositions(user.UserName).Positions));
            };
            
            // Gets the stock settings
            Get["/Settings"] = parameters =>
            {
                return this.Authenticate((user) =>
                {
                    StockSettings settings = Global.StockStore.GetStockSettings(user.UserName);
                    if (settings == null)
                    {
                        settings = new StockSettings()
                        {
                            Id = user.UserName,
                            SellStockFee = 0.03f,
                            TradeCommission = 7.95f
                        };

                        Global.StockStore.SaveStockSettings(settings);
                    }

                    return this.Response.AsJson(settings);
                });
            };

            // Saves the stock settings.
            Post["/Settings"] = parameters =>
            {
                return this.Authenticate((user) =>
                {
                    StockSettings settings = this.Bind<StockSettings>();
                    settings.Id = user.UserName;

                    if (settings != null)
                    {
                        Global.StockStore.UpdateStockSettings(settings);
                        return this.Response.AsText(string.Empty);
                    }
                    
                    return this.Response.AsJson("Unable to process request!", HttpStatusCode.BadRequest);
                });
            };
        }

        private static ActivePositions GetOrCreateActivePositions(string userName)
        {
            ActivePositions activePositions = Global.StockStore.GetActivePositions(userName);
            if (activePositions == null)
            {
                activePositions = new ActivePositions()
                {
                    Id = userName,
                    Positions = new List<ActiveStock>()
                };

                Global.StockStore.SaveActivePositions(activePositions);
            }

            return activePositions;
        }
    }
}
