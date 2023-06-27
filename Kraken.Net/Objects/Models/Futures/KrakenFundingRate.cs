using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace Kraken.Net.Objects.Models.Futures
{
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
