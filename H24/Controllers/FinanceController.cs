using H24.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace H24.Modules
{
    /// <summary>
    /// Module for retrieving the status of various server resources.
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class FinanceController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly AppSettings appSettings;
        private readonly IHttpClientFactory httpClientFactory;

        public FinanceController(ILogger<StatusController> logger, IOptions<AppSettings> appSettings, IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.appSettings = appSettings.Value;
            this.httpClientFactory = httpClientFactory;
        }

        // Ideally, this would be it's own separate, isolated logic, but I'm keeping it here so all the finance stuff is reasonably consolidated.
        // May throw, should be done in a nerror handling block
        public async Task<Dictionary<string, float>> GetQuotesAsync()
        {
            Dictionary<string, float> symbolsWithQuotes = new Dictionary<string, float>();

            // For testing, enable this block

            // await Task.Delay(0).ConfigureAwait(false);
            // symbolsWithQuotes.Add("DGRO", 47.54f);
            // symbolsWithQuotes.Add("EMXC", 59.24f);
            // symbolsWithQuotes.Add("IEFA", 72.7f);
            // symbolsWithQuotes.Add("IEMG", 65.16f);
            // symbolsWithQuotes.Add("IXUS", 70.96f);
            // symbolsWithQuotes.Add("MSFT", 230.35f);
            // symbolsWithQuotes.Add("MUB", 115.63f);
            // symbolsWithQuotes.Add("SCHB", 95.91f);
            // symbolsWithQuotes.Add("SCHH", 40.24f);
            // symbolsWithQuotes.Add("VCIT", 92.62f);
            // symbolsWithQuotes.Add("VOO", 359.23f);
            // symbolsWithQuotes.Add("VTWO", 183.14f);
            // symbolsWithQuotes.Add("VYM", 99.6636f);
            // return symbolsWithQuotes;

            string symbols = string.Join(',', appSettings.AssetAllocations.Keys);
            string url = $"https://fastquote.fidelity.com/service/quote/nondisplay/json?productid=research&symbols={symbols}&quotetype=D";

            using (HttpResponseMessage response = await this.httpClientFactory.CreateClient().GetAsync(url).ConfigureAwait(false))
            {
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                content = content.Trim(new[] { '(', ')' }); // Remove weird leading/trailing parentheses
                
                // For each SYMBOL_RESPONSE, replace with a number to actually be able to parse this silly almost-JSON.
                int counter = 0;
                while (content.Contains("SYMBOL_RESPONSE"))
                {
                    content = new Regex("SYMBOL_RESPONSE").Replace(content, "SR" + counter, 1);
                    ++counter;
                }

                var parsedJson = JObject.Parse(content);
                foreach (var token in parsedJson.SelectTokens("$.QUOTE.*").Where(token => token.Path.Contains("QUOTE.SR")))
                {
                    string symbol = token.SelectToken("SYMBOL").Value<string>();
                    float quote = float.Parse(token.SelectToken("QUOTE_DATA.LAST_PRICE").Value<string>());
                    symbolsWithQuotes.Add(symbol, quote);
                }
            }

            return symbolsWithQuotes;
        }

        public Dictionary<string, float> ComputeAllocations(Dictionary<string, float> stockPrices)
        {
            float totalValue = appSettings.AssetAllocations.Sum(asset => asset.Value * stockPrices[asset.Key]);

            // Allocation = for each asset in category, sum the asset price by the amount / total value of all assets
            return appSettings.AssetsInCategories.ToDictionary(category => category.Key, category =>
                100.0f * category.Value.Sum(asset => appSettings.AssetAllocations[asset] * stockPrices[asset]) / totalValue);
        }

        public Dictionary<string, float> ComputePerformances(Dictionary<string, float> stockPrices)
        {
            return appSettings.AssetsInCategories.ToDictionary(category => category.Key, category =>
                -100.0f * (1 - (category.Value.Sum(asset => appSettings.AssetAllocations[asset] * stockPrices[asset]) /
                category.Value.Sum(asset => appSettings.AssetAllocations[asset] * appSettings.AssetCostBasis[asset]))));
        }

        public float ComputeOverallPerformance(Dictionary<string, float> stockPrices)
        {
            List<string> assets = appSettings.AssetsInCategories.Values.SelectMany(asset => asset).ToList();
            return -100.0f * (1 - (assets.Sum(asset => appSettings.AssetAllocations[asset] * stockPrices[asset]) /
                assets.Sum(asset => appSettings.AssetAllocations[asset] * appSettings.AssetCostBasis[asset])));
        }

        public IActionResult Status()
        {
            this.logger.LogInformation($"Returned financial information");
            var quotes = GetQuotesAsync().GetAwaiter().GetResult();
            return this.Ok(
                new
                {
                    allocations = ComputeAllocations(quotes),
                    performances = ComputePerformances(quotes),
                    overalPerformance = ComputeOverallPerformance(quotes)
                });
        }
    }
}