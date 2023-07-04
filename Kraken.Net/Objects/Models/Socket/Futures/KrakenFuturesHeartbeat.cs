using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Heartbeat
    /// </summary>
    public class KrakenFuturesHeartbeat : KrakenFuturesUpdateMessage
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonProperty("time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
    }
}
