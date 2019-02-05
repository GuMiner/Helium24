using H24.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace H24.Modules
{
    /// <summary>
    /// Module for retrieving the status of various server resources.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class QuotesController : ControllerBase
    {
        private readonly IOptions<AppSettings> settings;
        private readonly ILogger logger;

        public QuotesController(IOptions<AppSettings> settings, ILogger<QuotesController> logger)
        {
            this.settings = settings;
            this.logger = logger;
        }

        public IActionResult Group()
        {
            IQueryCollection query = this.Request.Query;
            Dictionary<string, float> tickerValues = query.Where(kvp => kvp.Key.Equals(kvp.Value)).ToDictionary(kvp => kvp.Key.ToLower(), kvp => 0.03f);

            QuoteWrapper.GetTickerValues(logger, settings, tickerValues);
            return this.Ok(new
            {
                TickerValues = tickerValues
            });
        }
    }
}