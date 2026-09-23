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
            SpotUserDataTrackerConfig? config) : base(
                logger,
                restClient.SpotApi.SharedApi,
                restClient.SpotApi.SharedApi,
                socketClient.SpotApi.SharedApi,

                restClient.SpotApi.SharedApi,
                restClient.SpotApi.SharedApi,
                socketClient.SpotApi.SharedApi,

                restClient.SpotApi.SharedApi,
                null,
                userIdentifier,
                config ?? new SpotUserDataTrackerConfig())
        {
        }
    }

    ///// <inheritdoc/>
    //public class KrakenUserFuturesDataTracker : UserFuturesDataTracker
    //{
    //    /// <inheritdoc/>
    //    protected override bool WebsocketPositionUpdatesAreFullSnapshots => false;

    //    /// <summary>
    //    /// ctor
    //    /// </summary>
    //    public KrakenUserFuturesDataTracker(
    //        ILogger<KrakenUserFuturesDataTracker> logger,
    //        IKrakenRestClient restClient,
    //        IKrakenSocketClient socketClient,
    //        string? userIdentifier,
    //        FuturesUserDataTrackerConfig? config) : base(logger,
    //            restClient.FuturesApi.SharedApi,

    //            restClient.FuturesApi.SharedApi,
    //            socketClient.FuturesApi.SharedApi,

    //            restClient.FuturesApi.SharedApi,
    //            null,
    //            socketClient.FuturesApi.SharedApi,

    //            restClient.FuturesApi.SharedApi,
    //            socketClient.FuturesApi.SharedApi,

    //            restClient.FuturesApi.SharedApi,
    //            socketClient.FuturesApi.SharedApi,

    //            userIdentifier,
    //            config ?? new FuturesUserDataTrackerConfig())
    //    {
    //    }
    //}
}
