using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Futures
{
    internal record KrakenFuturesLeverageResult : KrakenFuturesResult<IEnumerable<KrakenFuturesLeverage>>
    {
        [JsonProperty("leveragePreferences")]
        public override IEnumerable<KrakenFuturesLeverage> Data { get; set; } = Array.Empty<KrakenFuturesLeverage>();
    }

    /// <summary>
    /// Leverage setting
    /// </summary>
    public record KrakenFuturesLeverage
    {
        /// <summary>
        /// Symbol
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Max leverage
        /// </summary>
        public decimal MaxLeverage { get; set; }
    }
}
