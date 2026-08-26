
using CryptoExchange.Net.Interfaces.Clients;

namespace Kraken.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Spot API endpoints
    /// </summary>
    public interface IKrakenRestClientSpotApi : IRestApiClient<KrakenCredentials>, IDisposable
    {
        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        /// <see cref="IKrakenRestClientSpotApiAccount"/>
        IKrakenRestClientSpotApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        /// <see cref="IKrakenRestClientSpotApiExchangeData"/>
        IKrakenRestClientSpotApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        /// <see cref="IKrakenRestClientSpotApiTrading"/>
        IKrakenRestClientSpotApiTrading Trading { get; }

        /// <summary>
        /// Endpoints related to Kraken Earn
        /// </summary>
        /// <see cref="IKrakenRestClientSpotApiEarn"/>
        IKrakenRestClientSpotApiEarn Earn { get; }

        /// <summary>
        /// Get the shared rest requests client. For new implementations prefer using <see cref="SharedApi"/>
        /// </summary>
        public IKrakenRestClientSpotApiShared SharedClient { get; }
        /// <summary>
        /// Gets the aggregate Shared API interface. Shared APIs provide a common,
        /// exchange-independent contract for accessing functionality across different
        /// exchange client libraries.
        /// </summary>
        public IKrakenRestClientSpotSharedApi SharedApi { get; }
    }
}