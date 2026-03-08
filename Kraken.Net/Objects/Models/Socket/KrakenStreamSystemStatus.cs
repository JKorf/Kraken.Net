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
        /// ["<c>connection_id</c>"] Connection id
        /// </summary>
        [JsonPropertyName("connection_id")]
        public long ConnectionId { get; set; }
        /// <summary>
        /// ["<c>system</c>"] Status
        /// </summary>

        [JsonPropertyName("system")]
        public SystemStatus Status { get; set; }
        /// <summary>
        /// ["<c>version</c>"] Version
        /// </summary>
        [JsonPropertyName("version")]
        public string Version { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>api_version</c>"] API Version
        /// </summary>
        [JsonPropertyName("api_version")]
        public string ApiVersion { get; set; } = string.Empty;
    }
}
