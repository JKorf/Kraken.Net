using CryptoExchange.Net.Interfaces.CommonClients;

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
        IKrakenRestClientSpotApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        IKrakenRestClientSpotApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        IKrakenRestClientSpotApiTrading Trading { get; }

        /// <summary>
        /// Endpoints related to Kraken Earn
        /// </summary>
        IKrakenRestClientSpotApiEarn Earn { get; }

        /// <summary>
        /// DEPRECATED; use <see cref="CryptoExchange.Net.SharedApis.ISharedClient" /> instead for common/shared functionality. See <see href="SHAREDDOCSURL" /> for more info.
        /// </summary>
        public ISpotClient CommonSpotClient { get; }

        /// <summary>
        /// Get the shared rest requests client
        /// </summary>
        public IKrakenRestClientSpotApiShared SharedClient { get; }
    }
}