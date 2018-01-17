using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace Helium24
{
    /// <summary>
    /// Abstracts away retrieving quotes.
    /// </summary>
    public class QuoteWrapper
    {
        /// <summary>
        /// Fills in the ticker values of all provided tickers, or null on an error.
        /// </summary>
        /// <param name="tickers">The tickers to get ticker values for.</param>
        public static void GetTickerValues(Dictionary<string, float> tickers)
        {
            foreach (string ticker in tickers.Keys.ToList())
            {
                tickers[ticker] = 0.01f;
            }

            try
            {
                Random wiggleDelay = new Random();
                foreach (string ticker in tickers.Keys.ToList())
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string uri = string.Format(ConfigurationManager.AppSettings["QuoteUri"], ticker);

                        // Perform the web call.
                        HttpResponseMessage response = client.GetAsync(uri).GetAwaiter().GetResult();
                        if (!response.IsSuccessStatusCode)
                        {
                            continue;
                        }

                        string responseText = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                        // Extract out the ticker value efficiently.
                        string lastTradeStr = "Last Trade [tick]</td>";
                        int lastTradeIdx = responseText.IndexOf(lastTradeStr);
                        if (lastTradeIdx == -1)
                        {
                            continue;
                        }

                        int quoteStartIdx = responseText.IndexOf('>', lastTradeIdx + lastTradeStr.Length) + 1;
                        if (quoteStartIdx == -1)
                        {
                            continue;
                        }

                        int quoteEndIdx = responseText.IndexOf('<', quoteStartIdx);
                        if (quoteEndIdx == -1)
                        {
                            continue;
                        }

                        string tickerValue = responseText.Substring(quoteStartIdx, quoteEndIdx - quoteStartIdx).Trim();
                        float tickerValueDecoded;
                        if (!float.TryParse(tickerValue, out tickerValueDecoded))
                        {
                            continue;
                        }

                        tickers[ticker] = tickerValueDecoded;
                        Global.Log($"Read {tickerValueDecoded} for {ticker}");
                    }

                    // Sleep a bit to not overload Fidelity
                    Thread.Sleep(10 * wiggleDelay.Next(1, 5));
                }
            }
            catch (Exception ex)
            {
                Global.Log($"Unable to read tickers: {ex.Message}");
            }
        }
    }
}
