namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// Socket token
    /// </summary>
    public class KrakenWebSocketToken
    {
        /// <summary>
        /// Token to use for connecting to private websockets
        /// </summary>
        public string Token { get; set; } = string.Empty;
        /// <summary>
        /// Expires after x seconds
        /// </summary>
        public int Expires { get; set; }
    }
}
