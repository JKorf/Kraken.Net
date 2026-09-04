using Kraken.Net.Interfaces.Clients;
using Kraken.Net.Interfaces.Clients.FuturesApi;
using Kraken.Net.Interfaces.Clients.SpotApi;

namespace Kraken.Net.Clients
{
    /// <inheritdoc />
    public class KrakenSharedApiClient : IKrakenSharedApiClient
    {
        /// <inheritdoc />
        public IKrakenRestClientSpotSharedApi SpotRest { get; }
        /// <inheritdoc />
        public IKrakenRestClientFuturesSharedApi FuturesRest { get; }
        /// <inheritdoc />
        public IKrakenSocketClientSpotSharedApi SpotSocket { get; }
        /// <inheritdoc />
        public IKrakenSocketClientFuturesSharedApi FuturesSocket { get; }

        /// <summary>
        /// ctor
        /// </summary>
        public KrakenSharedApiClient(
            IKrakenRestClient restClient,
            IKrakenSocketClient socketClient)
        {
            SpotRest = restClient.SpotApi.SharedApi;
            FuturesRest = restClient.FuturesApi.SharedApi;
            SpotSocket = socketClient.SpotApi.SharedApi;
            FuturesSocket = socketClient.FuturesApi.SharedApi;
        }
    }
}
