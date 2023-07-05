using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Futures
{
    internal class KrakenFundingRatesResult : KrakenFuturesResult<IEnumerable<KrakenFundingRate>>
    {
        [JsonProperty("rates")]
        public override IEnumerable<KrakenFundingRate> Data { get; set; } = Array.Empty<KrakenFundingRate>();
    }

    /// <summary>
    /// Funding rate info
    /// </summary>
    public class KrakenFundingRate
    {
        /// <summary>
        /// Funding rate
        /// </summary>
        public decimal FundingRate { get; set; }
        /// <summary>
        /// Relative funding rate
        /// </summary>
        public decimal RelativeFundingRate { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
    }
}
