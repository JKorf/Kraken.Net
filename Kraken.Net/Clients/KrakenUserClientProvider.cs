using CryptoExchange.Net.Clients;
using Kraken.Net.Interfaces.Clients;
using Kraken.Net.Objects.Options;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace Kraken.Net.Clients
{
    /// <inheritdoc />
    public class KrakenUserClientProvider : UserClientProvider<
        IKrakenRestClient,
        IKrakenSocketClient,
        KrakenRestOptions,
        KrakenSocketOptions,
        KrakenCredentials,
        KrakenEnvironment
        >, IKrakenUserClientProvider
    {
       
        /// <inheritdoc />
        public override string ExchangeName => KrakenExchange.ExchangeName;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="optionsDelegate">Options to use for created clients</param>
        public KrakenUserClientProvider(Action<KrakenOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate).Rest), Options.Create(ApplyOptionsDelegate(optionsDelegate).Socket))
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        public KrakenUserClientProvider(
            HttpClient? httpClient,
            ILoggerFactory? loggerFactory,
            IOptions<KrakenRestOptions> restOptions,
            IOptions<KrakenSocketOptions> socketOptions)
            : base(httpClient, loggerFactory, restOptions, socketOptions)
        {
        }

        /// <inheritdoc />
        protected override IKrakenRestClient ConstructRestClient(HttpClient client, ILoggerFactory? loggerFactory, IOptions<KrakenRestOptions> options)
            => new KrakenRestClient(client, loggerFactory, options);
        /// <inheritdoc />
        protected override IKrakenSocketClient ConstructSocketClient(ILoggerFactory? loggerFactory, IOptions<KrakenSocketOptions> options) 
            => new KrakenSocketClient(options, loggerFactory);
    }
}
