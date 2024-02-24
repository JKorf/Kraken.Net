using CryptoExchange.Net.Converters;
using Kraken.Net.Objects.Sockets;
using Newtonsoft.Json;
using System;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Heartbeat
    /// </summary>
    public class KrakenFuturesHeartbeatUpdate : KrakenFuturesEvent
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonProperty("time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
    }
}
