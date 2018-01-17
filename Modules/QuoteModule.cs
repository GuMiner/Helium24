using Nancy;
using System.Collections.Generic;
using System.Linq;

namespace Helium24.Modules
{
    /// <summary>
    /// Retrieves a block of quotes given their tickers.
    /// </summary>
    public class QuoteModule : NancyModule
    {
        public QuoteModule()
            : base("/Quotes")
        {
            // Gets the general stock status and transfers information.
            Get["/Group"] = parameters =>
            {
                return this.Authenticate((user) =>
                {
                    DynamicDictionary query = this.Request.Query;
                    Dictionary<string, float> tickerValues = query.ToDictionary().Where(kvp => kvp.Key.Equals(kvp.Value)).ToDictionary(kvp => kvp.Key.ToLower(), kvp => 0.03f);
                    
                    QuoteWrapper.GetTickerValues(tickerValues);
                    return this.Response.AsJson(new
                    {
                        TickerValues = tickerValues
                    }); 
                });
            };
        }
    }
}
