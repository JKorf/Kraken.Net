using CryptoExchange.Net.Objects;
using Kraken.Net.Interfaces;

namespace Kraken.Net
{
    /// <summary>
    /// Options for the Kraken client
    /// </summary>
    public class KrakenClientOptions : RestClientOptions
    {
        /// <summary>
        /// The static password configured as two-factor authentication for the API key. Will be send as otp parameter on private requests.
        /// </summary>
        public string StaticTwoFactorAuthenticationPassword { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        public KrakenClientOptions(): base("https://api.kraken.com")
        {
        }
    }

    /// <summary>
    /// Options for the Kraken socket client
    /// </summary>
    public class KrakenSocketClientOptions : SocketClientOptions
    {
        /// <summary>
        /// ctor
        /// </summary>
        public KrakenSocketClientOptions(): base("wss://ws.kraken.com")
        {
            SocketSubscriptionsCombineTarget = 10;
        }
    }

    /// <summary>
    /// Options for the Kraken symbol order book
    /// </summary>
    public class KrakenOrderBookOptions : OrderBookOptions
    {
        /// <summary>
        /// The client to use for the socket connection. When using the same client for multiple order books the connection can be shared.
        /// </summary>
        public IKrakenSocketClient? SocketClient { get; }

        /// <summary>
        /// </summary>
        /// <param name="client">The client to use for the socket connection. When using the same client for multiple order books the connection can be shared.</param>
        public KrakenOrderBookOptions(IKrakenSocketClient? client = null) : base("Kraken", false, true)
        {
            SocketClient = client;
        }
    }
}

