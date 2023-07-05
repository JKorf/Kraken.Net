using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Futures
{
    internal class KrakenFuturesPnlCurrencyResult : KrakenFuturesResult<IEnumerable<KrakenFuturesPnlCurrency>>
    {
        [JsonProperty("preferences")]
        public override IEnumerable<KrakenFuturesPnlCurrency> Data { get; set; } = Array.Empty<KrakenFuturesPnlCurrency>();
    }

    /// <summary>
    /// Profit and loss currency preference
    /// </summary>
    public class KrakenFuturesPnlCurrency
    {
        /// <summary>
        /// Profit and loss currency
        /// </summary>
        public string PnlCurrency { get; set; } = string.Empty;
        /// <summary>
        /// Symbol
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
    }
}
