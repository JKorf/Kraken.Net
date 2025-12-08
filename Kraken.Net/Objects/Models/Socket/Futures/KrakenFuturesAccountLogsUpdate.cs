using Kraken.Net.Objects.Models.Futures;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Account log snapshot update
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesAccountLogsSnapshotUpdate : KrakenFuturesSocketMessage
    {
        /// <summary>
        /// Account logs
        /// </summary>
        [JsonPropertyName("logs")]
        public KrakenAccountLog[] Logs { get; set; } = Array.Empty<KrakenAccountLog>();
    }

    /// <summary>
    /// New account log update
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesAccountLogsUpdate : KrakenFuturesSocketMessage
    {
        /// <summary>
        /// New entry
        /// </summary>
        [JsonPropertyName("new_entry")]
        public KrakenAccountLog NewEntry { get; set; } = null!;
    }
}
