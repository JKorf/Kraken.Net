using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// System status
    /// </summary>
    [SerializationModel]
    public record KrakenStreamSystemStatus
    {
        /// <summary>
        /// Connection id
        /// </summary>
        [JsonPropertyName("connection_id")]
        public long ConnectionId { get; set; }
        /// <summary>
        /// Status
        /// </summary>

        [JsonPropertyName("system")]
        public SystemStatus Status { get; set; }
        /// <summary>
        /// Version
        /// </summary>
        [JsonPropertyName("version")]
        public string Version { get; set; } = string.Empty;
        /// <summary>
        /// API Version
        /// </summary>
        [JsonPropertyName("api_version")]
        public string ApiVersion { get; set; } = string.Empty;
    }
}
