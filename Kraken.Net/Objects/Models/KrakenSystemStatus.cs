using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// System status
    /// </summary>
    public record KrakenSystemStatus
    {
        /// <summary>
        /// Platform status
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        [JsonPropertyName("status")]
        public SystemStatus Status { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
