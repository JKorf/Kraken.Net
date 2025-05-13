using Kraken.Net.Interfaces.Clients.SpotApi;

namespace Kraken.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// Futures API endpoints
    /// </summary>
    public interface IKrakenRestClientFuturesApi : IRestApiClient, IDisposable
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
        /// Get the shared rest requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        public IKrakenRestClientFuturesApiShared SharedClient { get; }
    }
}