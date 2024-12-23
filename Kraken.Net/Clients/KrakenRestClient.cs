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
    /// <inheritdoc cref="IKrakenRestClient" />
    public class KrakenRestClient : BaseRestClient, IKrakenRestClient
    {
        #region fields
        #endregion

        #region Api clients
        /// <inheritdoc />
        public IKrakenRestClientSpotApi SpotApi { get; }
        /// <inheritdoc />
        public IKrakenRestClientFuturesApi FuturesApi { get; }
        #endregion

        #region ctor
        /// <summary>
        /// Create a new instance of the KrakenRestClient using provided options
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public KrakenRestClient(Action<KrakenRestOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate)))
        {
        }

        /// <summary>
        /// Create a new instance of the KrakenRestClient
        /// </summary>
        /// <param name="options">Option configuration</param>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="httpClient">Http client for this client</param>
        public KrakenRestClient(HttpClient? httpClient, ILoggerFactory? loggerFactory, IOptions<KrakenRestOptions> options)
            : base(loggerFactory, "Kraken")
        {
            Initialize(options.Value);

            SpotApi = AddApiClient(new KrakenRestClientSpotApi(_logger, httpClient, options.Value));
            FuturesApi = AddApiClient(new KrakenRestClientFuturesApi(_logger, httpClient, options.Value));
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
        public static void SetDefaultOptions(Action<KrakenRestOptions> optionsDelegate)
        {
            KrakenRestOptions.Default = ApplyOptionsDelegate(optionsDelegate);
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
