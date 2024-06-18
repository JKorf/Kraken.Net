using Kraken.Net.Objects.Sockets;

namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// Cancel all result
    /// </summary>
    public record KrakenStreamCancelAllResult : KrakenQueryEvent
    {
        /// <summary>
        /// Number of orders canceled
        /// </summary>
        public int Count { get; set; }
    }
}
