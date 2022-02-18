namespace Kraken.Net.Objects
{
    /// <summary>
    /// Api addresses usable for the Kraken clients
    /// </summary>
    public class KrakenApiAddresses
    {
        /// <summary>
        /// The address used by the KrakenClient for the rest API
        /// </summary>
        public string RestClientAddress { get; set; } = "";
        /// <summary>
        /// The address used by the KrakenSocketClient for the public socket API
        /// </summary>
        public string SocketClientPublicAddress { get; set; } = "";
        /// <summary>
        /// The address used by the KrakenSocketClient for the private socket API
        /// </summary>
        public string SocketClientPrivateAddress { get; set; } = "";

        /// <summary>
        /// The default addresses to connect to the Kraken.com API
        /// </summary>
        public static KrakenApiAddresses Default = new KrakenApiAddresses
        {
            RestClientAddress = "https://api.kraken.com",
            SocketClientPublicAddress = "wss://ws.kraken.com",
            SocketClientPrivateAddress = "wss://ws-auth.kraken.com/"
        };
    }
}
