using CryptoExchange.Net.Converters.SystemTextJson;
using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// System status
    /// </summary>
    [SerializationModel]
    public record KrakenSystemStatus
    {
        /// <summary>
        /// Platform status
        /// </summary>

        [JsonPropertyName("status")]
        public SystemStatus Status { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
