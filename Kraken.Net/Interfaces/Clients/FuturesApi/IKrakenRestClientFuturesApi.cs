using CryptoExchange.Net.Interfaces.Clients;

namespace Kraken.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// Futures API endpoints
    /// </summary>
    public interface IKrakenRestClientFuturesApi : IRestApiClient<KrakenCredentials>, IDisposable
    {
        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        /// <see cref="IKrakenRestClientFuturesApiAccount"/>
        IKrakenRestClientFuturesApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        /// <see cref="IKrakenRestClientFuturesApiExchangeData"/>
        IKrakenRestClientFuturesApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        /// <see cref="IKrakenRestClientFuturesApiTrading"/>
        IKrakenRestClientFuturesApiTrading Trading { get; }

        /// <summary>
        /// [V1] Get the shared rest requests client. For new implementations prefer using <see cref="SharedApi"/>
        /// </summary>
        public IKrakenRestClientFuturesApiShared SharedClient { get; }
        /// <summary>
        /// [V2] Gets the aggregate Shared API interface. Shared APIs provide a common,
        /// exchange-independent contract for accessing functionality across different
        /// exchange client libraries.
        /// </summary>
        public IKrakenRestClientFuturesSharedApi SharedApi { get; }
    }
}