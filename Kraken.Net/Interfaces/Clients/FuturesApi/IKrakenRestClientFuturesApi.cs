using CryptoExchange.Net.Interfaces;
using Kraken.Net.Clients.FuturesApi;
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
        KrakenRestClientFuturesApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        KrakenRestClientFuturesApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        KrakenRestClientFuturesApiTrading Trading { get; }
    }
}