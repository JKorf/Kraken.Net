using CryptoExchange.Net.SharedApis;
using Kraken.Net.Interfaces.Clients;
using Kraken.Net.Interfaces.Clients.FuturesApi;
using Kraken.Net.Interfaces.Clients.SpotApi;
using Kraken.Net.Objects.Options;
using Microsoft.Extensions.Options;

namespace Kraken.Net.Clients
{
    /// <inheritdoc />
    public class KrakenSharedApiClient : SharedApiClientBase, IKrakenSharedApiClient
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
            IKrakenSocketClient socketClient,
            IOptions<KrakenOptions> options)
            : base(options.Value.SharedApi.PreferredTransport,
                    restClient.SpotApi.SharedApi,
                    restClient.FuturesApi.SharedApi,
                    socketClient.SpotApi.SharedApi,
                    socketClient.FuturesApi.SharedApi
                  )
        {
            SpotRest = restClient.SpotApi.SharedApi;
            FuturesRest = restClient.FuturesApi.SharedApi;
            SpotSocket = socketClient.SpotApi.SharedApi;
            FuturesSocket = socketClient.FuturesApi.SharedApi;
        }
    }
}
