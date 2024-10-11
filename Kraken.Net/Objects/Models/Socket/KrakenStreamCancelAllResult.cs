using Kraken.Net.Objects.Sockets;

namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// Cancel all result
    /// </summary>
    public record KrakenStreamCancelAllResult
    {
        /// <summary>
        /// Number of orders canceled
        /// </summary>
        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
}
