
using CryptoExchange.Net.Interfaces.Clients;

namespace Kraken.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Spot API endpoints
    /// </summary>
    public interface IKrakenRestClientSpotApi : IRestApiClient, IDisposable
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
        /// Get the shared rest requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        public IKrakenRestClientSpotApiShared SharedClient { get; }
    }
}