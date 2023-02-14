using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using Kraken.Net.Clients.SpotApi;
using Kraken.Net.Interfaces.Clients;
using Kraken.Net.Interfaces.Clients.SpotApi;
using Kraken.Net.Objects;

namespace Kraken.Net.Clients
{
    /// <inheritdoc cref="IKrakenSocketClient" />
    public class KrakenSocketClient : BaseSocketClient, IKrakenSocketClient
    {
        #region Api clients

        /// <inheritdoc />
        public IKrakenSocketClientSpotStreams SpotStreams { get; }

        #endregion

        #region ctor
        /// <summary>
        /// Create a new instance of KrakenSocketClient using the default options
        /// </summary>
        public KrakenSocketClient() : this(KrakenSocketClientOptions.Default)
        {
        }

        /// <summary>
        /// Create a new instance of KrakenSocketClient using provided options
        /// </summary>
        /// <param name="options">The options to use for this client</param>
        public KrakenSocketClient(KrakenSocketClientOptions options) : base("Kraken", options)
        {
            SpotStreams = AddApiClient(new KrakenSocketClientSpotStreams(log, options));
        }
        #endregion

        #region methods

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="options">Options to use as default</param>
        public static void SetDefaultOptions(KrakenSocketClientOptions options)
        {
            KrakenSocketClientOptions.Default = options;
        }

        /// <inheritdoc />
        public void SetApiCredentials(ApiCredentials apiCredentials)
        {
            SpotStreams.SetApiCredentials(apiCredentials);
        }
        #endregion

    }
}
