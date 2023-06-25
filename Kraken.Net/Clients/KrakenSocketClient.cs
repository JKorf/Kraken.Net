using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using Kraken.Net.Clients.SpotApi;
using Kraken.Net.Interfaces.Clients;
using Kraken.Net.Interfaces.Clients.SpotApi;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Options;
using Microsoft.Extensions.Logging;
using System;

namespace Kraken.Net.Clients
{
    /// <inheritdoc cref="IKrakenSocketClient" />
    public class KrakenSocketClient : BaseSocketClient, IKrakenSocketClient
    {
        #region Api clients

        /// <inheritdoc />
        public IKrakenSocketClientSpotApi SpotApi { get; }

        #endregion

        #region ctor
        /// <summary>
        /// Create a new instance of the KrakenSocketClient
        /// </summary>
        /// <param name="loggerFactory">The logger</param>
        public KrakenSocketClient(ILoggerFactory? loggerFactory = null) : this((x) => { }, loggerFactory)
        {
        }

        /// <summary>
        /// Create a new instance of the KrakenSocketClient
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public KrakenSocketClient(Action<KrakenSocketOptions> optionsDelegate) : this(optionsDelegate, null)
        {
        }

        /// <summary>
        /// Create a new instance of the KrakenSocketClient
        /// </summary>
        /// <param name="loggerFactory">The logger</param>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public KrakenSocketClient(Action<KrakenSocketOptions> optionsDelegate, ILoggerFactory? loggerFactory = null) : base(loggerFactory, "Kraken")
        {
            var options = KrakenSocketOptions.Default.Copy();
            optionsDelegate(options);
            Initialize(options);

            SpotApi = AddApiClient(new KrakenSocketClientSpotApi(_logger, options));
        }
        #endregion

        #region methods

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<KrakenSocketOptions> optionsDelegate)
        {
            var options = KrakenSocketOptions.Default.Copy();
            optionsDelegate(options);
            KrakenSocketOptions.Default = options;
        }

        /// <inheritdoc />
        public void SetApiCredentials(ApiCredentials apiCredentials)
        {
            SpotApi.SetApiCredentials(apiCredentials);
        }
        #endregion

    }
}
