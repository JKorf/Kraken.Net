using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace Kraken.Net.Objects.Models.Futures
{
    internal record KrakenFuturesCancelAfterResult : KrakenFuturesResult<KrakenFuturesCancelAfter>
    {
        [JsonProperty("status")]
        public override KrakenFuturesCancelAfter Data { get; set; } = null!;
    }

    /// <summary>
    /// Cancel after info
    /// </summary>
    public record KrakenFuturesCancelAfter
    {
        /// <summary>
        /// Current timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime CurrentTime { get; set; }
        /// <summary>
        /// Trigger time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? TriggerTime { get; set; }
    }
}
