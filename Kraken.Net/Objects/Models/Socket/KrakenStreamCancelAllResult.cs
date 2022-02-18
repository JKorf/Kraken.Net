namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// Cancel all result
    /// </summary>
    public class KrakenStreamCancelAllResult : KrakenSocketResponseBase
    {
        /// <summary>
        /// Number of orders canceled
        /// </summary>
        public int Count { get; set; }
    }
}
