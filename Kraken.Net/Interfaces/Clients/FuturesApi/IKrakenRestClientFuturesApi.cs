using CryptoExchange.Net.Interfaces;
using Kraken.Net.Interfaces.Clients.SpotApi;
using System;

namespace Kraken.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// Futuers API endpoints
    /// </summary>
    public interface IKrakenRestClientFuturesApi : IRestApiClient, IDisposable
    {
        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        IKrakenRestClientFuturesApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        IKrakenRestClientFuturesApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        IKrakenRestClientFuturesApiTrading Trading { get; }

        /// <summary>
        /// Get the shared rest requests client. This interface is shared with other exhanges to allow for a common implementation for different exchanges.
        /// </summary>
        public IKrakenRestClientFuturesApiShared SharedClient { get; }
    }
}