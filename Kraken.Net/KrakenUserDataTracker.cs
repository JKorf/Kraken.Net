using Kraken.Net.Interfaces.Clients;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Trackers.UserData;
using CryptoExchange.Net.Trackers.UserData.Objects;
using Microsoft.Extensions.Logging;

namespace Kraken.Net
{
    /// <inheritdoc/>
    public class KrakenUserSpotDataTracker : UserSpotDataTracker
    {
        /// <summary>
        /// ctor
        /// </summary>
        public KrakenUserSpotDataTracker(
            ILogger<KrakenUserSpotDataTracker> logger,
            IKrakenRestClient restClient,
            IKrakenSocketClient socketClient,
            string? userIdentifier,
            SpotUserDataTrackerConfig config) : base(
                logger,
                restClient.SpotApi.SharedClient,
                null,
                restClient.SpotApi.SharedClient,
                socketClient.SpotApi.SharedClient,
                restClient.SpotApi.SharedClient,
                socketClient.SpotApi.SharedClient,
                null,
                userIdentifier,
                config)
        {
        }
    }

    /// <inheritdoc/>
    public class KrakenUserFuturesDataTracker : UserFuturesDataTracker
    {
        /// <inheritdoc/>
        protected override bool WebsocketPositionUpdatesAreFullSnapshots => false;

        /// <summary>
        /// ctor
        /// </summary>
        public KrakenUserFuturesDataTracker(
            ILogger<KrakenUserFuturesDataTracker> logger,
            IKrakenRestClient restClient,
            IKrakenSocketClient socketClient,
            string? userIdentifier,
            FuturesUserDataTrackerConfig config) : base(logger,
                restClient.FuturesApi.SharedClient,
                null,
                restClient.FuturesApi.SharedClient,
                socketClient.FuturesApi.SharedClient,
                restClient.FuturesApi.SharedClient,
                socketClient.FuturesApi.SharedClient,
                socketClient.FuturesApi.SharedClient,
                socketClient.FuturesApi.SharedClient,
                userIdentifier,
                config)
        {
        }
    }
}
