using System;
using System.Net.Http;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using Kraken.Net.Clients.SpotApi;
using Kraken.Net.Interfaces.Clients;
using Kraken.Net.Interfaces.Clients.SpotApi;
using Kraken.Net.Objects.Options;
using Microsoft.Extensions.Logging;

namespace Kraken.Net.Clients
{
    /// <inheritdoc cref="IKrakenRestClient" />
    public class KrakenRestClient : BaseRestClient, IKrakenRestClient
    {
        #region fields
        #endregion

        #region Api clients
        /// <inheritdoc />
        public IKrakenClientSpotApi SpotApi { get; }
        #endregion

        #region ctor
        /// <summary>
        /// Create a new instance of the KrakenRestClient using provided options
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public KrakenRestClient(Action<KrakenRestOptions> optionsDelegate) : this(null, null, optionsDelegate)
        {
        }

        /// <summary>
        /// Create a new instance of the KrakenRestClient using default options
        /// </summary>
        public KrakenRestClient(ILoggerFactory? loggerFactory = null, HttpClient? httpClient = null) : this(httpClient, loggerFactory, null)
        {
        }

        /// <summary>
        /// Create a new instance of the KrakenRestClient
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="httpClient">Http client for this client</param>
        public KrakenRestClient(HttpClient? httpClient, ILoggerFactory? loggerFactory, Action<KrakenRestOptions>? optionsDelegate = null)
            : base(loggerFactory, "Kraken")
        {
            var options = KrakenRestOptions.Default.Copy();
            if (optionsDelegate != null)
                optionsDelegate(options);
            Initialize(options);

            SpotApi = AddApiClient(new KrakenRestClientSpotApi(_logger, httpClient, options));
        }
        #endregion

        #region methods

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<KrakenRestOptions> optionsDelegate)
        {
            var options = KrakenRestOptions.Default.Copy();
            optionsDelegate(options);
            KrakenRestOptions.Default = options;
        }

        /// <inheritdoc />
        public void SetApiCredentials(ApiCredentials apiCredentials)
        {
            SpotApi.SetApiCredentials(apiCredentials);
        }
        #endregion

    }
}
