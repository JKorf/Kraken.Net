using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects.Options;

namespace Kraken.Net.Objects.Options
{
    /// <summary>
    /// Options for the KrakenSocketClient
    /// </summary>
    public class KrakenSocketOptions : SocketExchangeOptions<KrakenEnvironment>
    {
        /// <summary>
        /// Default options for new KrakenRestClients
        /// </summary>
        public static KrakenSocketOptions Default { get; set; } = new KrakenSocketOptions()
        {
            SocketSubscriptionsCombineTarget = 10,
            Environment = KrakenEnvironment.Live
        };

        /// <summary>
        /// Optional nonce provider for signing requests. Careful providing a custom provider; once a nonce is sent to the server, every request after that needs a higher nonce than that
        /// </summary>
        public INonceProvider? NonceProvider { get; set; }

        /// <summary>
        /// Options for the Spot API
        /// </summary>
        public SocketApiOptions SpotOptions { get; private set; } = new SocketApiOptions();

        /// <summary>
        /// Options for the Futures API
        /// </summary>
        public SocketApiOptions FuturesOptions { get; private set; } = new SocketApiOptions() { MaxSocketConnections = 100 };

        internal KrakenSocketOptions Copy()
        {
            var options = Copy<KrakenSocketOptions>();
            options.SpotOptions = SpotOptions.Copy<SocketApiOptions>();
            options.FuturesOptions = FuturesOptions.Copy<SocketApiOptions>();
            options.NonceProvider = NonceProvider;
            return options;
        }
    }
}
