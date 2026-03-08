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
        /// ["<c>status</c>"] Platform status
        /// </summary>

        [JsonPropertyName("status")]
        public SystemStatus Status { get; set; }
        /// <summary>
        /// ["<c>timestamp</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
