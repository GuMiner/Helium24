using H24.Definitions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace H24.Modules
{
    /// <summary>
    /// Module for retrieving the status of various server resources.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class StockController : ControllerBase
    {
        private readonly ILogger logger;

        public StockController(ILogger<StockController> logger)
        {
            this.logger = logger;
        }

        [HttpPost]
        public IActionResult Purchase([FromBody]ActiveStock activeStock)
        {
            string id = this.HttpContext.GetId();
            if (activeStock != null)
            {
                if (activeStock.PurchaseAmount < 0 || activeStock.PurchasePrice < 0)
                {
                    return this.BadRequest("The purchase amount and price must both be > 0.");
                }
                ActivePositions activePositions = GetOrCreateActivePositions(id);

                activePositions.Positions.Add(activeStock);
                activePositions.Positions.Sort((first, second) => first.DateAcquired.CompareTo(second.DateAcquired));
                Program.StockStore.UpdateActivePositions(activePositions);
                return this.Ok(string.Empty);
            }

            return this.BadRequest("Unable to process request!");
        }

        [HttpPost]
        public IActionResult Sale([FromBody]StockSale selection)
        {
            string id = this.HttpContext.GetId();
            ActivePositions activePositions = GetOrCreateActivePositions(id);

            if (selection.Amount <= 0)
            {
                return this.BadRequest("The amount to sell must be greater than zero!");
            }

            if (selection.SelectedIndex >= 0 && selection.SelectedIndex < activePositions.Positions.Count)
            {
                ActiveStock activeStock = activePositions.Positions[selection.SelectedIndex];

                if (Math.Abs(activeStock.PurchaseAmount - selection.Amount) < 0.00001f)
                {
                    activePositions.Positions.RemoveAt(selection.SelectedIndex);
                    Program.StockStore.UpdateActivePositions(activePositions);
                    return this.Ok(string.Empty);
                }
                else
                {
                    if (selection.Amount > activeStock.PurchaseAmount)
                    {
                        return this.BadRequest("The amount to sell must not be more than the purchased amount!");
                    }

                    activePositions.Positions.RemoveAt(selection.SelectedIndex);
                    activePositions.Positions.Add(ActiveStock.Split(activeStock, activeStock.PurchaseAmount - selection.Amount));
                    activePositions.Positions.Sort((first, second) => first.DateAcquired.CompareTo(second.DateAcquired));
                    Program.StockStore.UpdateActivePositions(activePositions);
                    return this.Ok(string.Empty);
                }
            }

            return this.BadRequest("Unable to process request!");
        }

        public IActionResult ActivePositions()
        {
            string id = this.HttpContext.GetId();
            return this.Ok(GetOrCreateActivePositions(id).Positions);
        }

        [HttpGet]
        public IActionResult Settings()
        {
            string id = this.HttpContext.GetId();
            StockSettings settings = Program.StockStore.GetStockSettings(id);
            if (settings == null)
            {
                settings = new StockSettings()
                {
                    Id = id,
                    SellStockFee = 0.03f,
                    TradeCommission = 7.95f
                };

                Program.StockStore.SaveStockSettings(settings);
            }

            return this.Ok(settings);
        }

        [HttpPost]
        public IActionResult Settings([FromBody]StockSettings settings)
        {
            string id = this.HttpContext.GetId();
            settings.Id = id;

            if (settings != null)
            {
                Program.StockStore.UpdateStockSettings(settings);
                return this.Ok(string.Empty);
            }

            return this.BadRequest("Unable to process request!");
        }

        private static ActivePositions GetOrCreateActivePositions(string userName)
        {
            ActivePositions activePositions = Program.StockStore.GetActivePositions(userName);
            if (activePositions == null)
            {
                activePositions = new ActivePositions()
                {
                    Id = userName,
                    Positions = new List<ActiveStock>()
                };

                Program.StockStore.SaveActivePositions(activePositions);
            }

            return activePositions;
        }
    }
}