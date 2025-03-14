using CryptoExchange.Net.Converters.SystemTextJson;
using Kraken.Net.Objects.Sockets;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Heartbeat
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesHeartbeatUpdate : KrakenFuturesEvent
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
    }
}
