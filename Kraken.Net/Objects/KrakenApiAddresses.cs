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
        public string SpotRestClientAddress { get; set; } = "";
        /// <summary>
        /// The address used by the KrakenSocketClient for the public socket API
        /// </summary>
        public string SpotSocketPublicAddress { get; set; } = "";
        /// <summary>
        /// The address used by the KrakenSocketClient for the private socket API
        /// </summary>
        public string SpotSocketPrivateAddress { get; set; } = "";

        /// <summary>
        /// The default addresses to connect to the Kraken.com API
        /// </summary>
        public static KrakenApiAddresses Default = new KrakenApiAddresses
        {
            SpotRestClientAddress = "https://api.kraken.com",
            SpotSocketPublicAddress = "wss://ws.kraken.com",
            SpotSocketPrivateAddress = "wss://ws-auth.kraken.com/"
        };
    }
}
