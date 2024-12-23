using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects.Options;
using Kraken.Net.Clients.FuturesApi;
using Kraken.Net.Clients.SpotApi;
using Kraken.Net.Interfaces.Clients;
using Kraken.Net.Interfaces.Clients.FuturesApi;
using Kraken.Net.Interfaces.Clients.SpotApi;
using Kraken.Net.Objects.Options;
using Microsoft.Extensions.Options;

namespace Kraken.Net.Clients
{
    /// <inheritdoc cref="IKrakenSocketClient" />
    public class KrakenSocketClient : BaseSocketClient, IKrakenSocketClient
    {
        #region Api clients

        /// <inheritdoc />
        public IKrakenSocketClientSpotApi SpotApi { get; }
        /// <inheritdoc />
        public IKrakenSocketClientFuturesApi FuturesApi { get; }

        #endregion

        #region ctor
        /// <summary>
        /// Create a new instance of the KrakenSocketClient
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public KrakenSocketClient(Action<KrakenSocketOptions>? optionsDelegate = null)
            : this(Options.Create(ApplyOptionsDelegate(optionsDelegate)), null)
        {
        }

        /// <summary>
        /// Create a new instance of the KrakenSocketClient
        /// </summary>
        /// <param name="loggerFactory">The logger</param>
        /// <param name="options">Option configuration</param>
        public KrakenSocketClient(IOptions<KrakenSocketOptions> options, ILoggerFactory? loggerFactory = null) : base(loggerFactory, "Kraken")
        {
            Initialize(options.Value);

            SpotApi = AddApiClient(new KrakenSocketClientSpotApi(_logger, options.Value));
            FuturesApi = AddApiClient(new KrakenSocketClientFuturesApi(_logger, options.Value));
        }
        #endregion

        #region methods

        /// <inheritdoc />
        public void SetOptions(UpdateOptions options)
        {
            SpotApi.SetOptions(options);
            FuturesApi.SetOptions(options);
        }

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<KrakenSocketOptions> optionsDelegate)
        {
            KrakenSocketOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }

        /// <inheritdoc />
        public void SetApiCredentials(ApiCredentials apiCredentials)
        {
            SpotApi.SetApiCredentials(apiCredentials);
            FuturesApi.SetApiCredentials(apiCredentials);
        }
        #endregion

    }
}
