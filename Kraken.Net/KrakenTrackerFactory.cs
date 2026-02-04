using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Trackers.Klines;
using CryptoExchange.Net.Trackers.Trades;
using CryptoExchange.Net.Trackers.UserData.Interfaces;
using CryptoExchange.Net.Trackers.UserData.Objects;
using Kraken.Net.Clients;
using Kraken.Net.Interfaces;
using Kraken.Net.Interfaces.Clients;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace Kraken.Net
{
    /// <inheritdoc />
    public class KrakenTrackerFactory : IKrakenTrackerFactory
    {
        private readonly IServiceProvider? _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        public KrakenTrackerFactory()
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public KrakenTrackerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public bool CanCreateKlineTracker(SharedSymbol symbol, SharedKlineInterval interval)
        {
            if (symbol.TradingMode != TradingMode.Spot)
                return false;

            var client = (_serviceProvider?.GetRequiredService<IKrakenSocketClient>() ?? new KrakenSocketClient());
            return client.SpotApi.SharedClient.SubscribeKlineOptions.IsSupported(interval);
        }

        /// <inheritdoc />
        public bool CanCreateTradeTracker(SharedSymbol symbol) => true;

        /// <inheritdoc />
        public IKlineTracker CreateKlineTracker(SharedSymbol symbol, SharedKlineInterval interval, int? limit = null, TimeSpan? period = null)
        {
            var restClient = (_serviceProvider?.GetRequiredService<IKrakenRestClient>() ?? new KrakenRestClient()).SpotApi.SharedClient;
            var socketClient = (_serviceProvider?.GetRequiredService<IKrakenSocketClient>() ?? new KrakenSocketClient()).SpotApi.SharedClient;

            return new KlineTracker(
                _serviceProvider?.GetRequiredService<ILoggerFactory>().CreateLogger(restClient.Exchange),
                restClient,
                socketClient,
                symbol,
                interval,
                limit,
                period
                );
        }
        /// <inheritdoc />
        public ITradeTracker CreateTradeTracker(SharedSymbol symbol, int? limit = null, TimeSpan? period = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<IKrakenRestClient>() ?? new KrakenRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<IKrakenSocketClient>() ?? new KrakenSocketClient();

            IRecentTradeRestClient? sharedRestClient;
            ITradeSocketClient sharedSocketClient;
            if (symbol.TradingMode == TradingMode.Spot)
            {
                sharedRestClient = restClient.SpotApi.SharedClient;
                sharedSocketClient = socketClient.SpotApi.SharedClient;
            }
            else
            {
                sharedRestClient = restClient.FuturesApi.SharedClient;
                sharedSocketClient = socketClient.FuturesApi.SharedClient;
            }

            return new TradeTracker(
                _serviceProvider?.GetRequiredService<ILoggerFactory>().CreateLogger(restClient.Exchange),
                sharedRestClient,
                null,
                sharedSocketClient,
                symbol,
                limit,
                period
                );
        }

        /// <inheritdoc />
        public IUserSpotDataTracker CreateUserSpotDataTracker(SpotUserDataTrackerConfig config)
        {
            var restClient = _serviceProvider?.GetRequiredService<IKrakenRestClient>() ?? new KrakenRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<IKrakenSocketClient>() ?? new KrakenSocketClient();
            return new KrakenUserSpotDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<KrakenUserSpotDataTracker>>() ?? new NullLogger<KrakenUserSpotDataTracker>(),
                restClient,
                socketClient,
                null,
                config
                );
        }

        /// <inheritdoc />
        public IUserSpotDataTracker CreateUserSpotDataTracker(string userIdentifier, SpotUserDataTrackerConfig config, ApiCredentials credentials, KrakenEnvironment? environment = null)
        {
            var clientProvider = _serviceProvider?.GetRequiredService<IKrakenUserClientProvider>() ?? new KrakenUserClientProvider();
            var restClient = clientProvider.GetRestClient(userIdentifier, credentials, environment);
            var socketClient = clientProvider.GetSocketClient(userIdentifier, credentials, environment);
            return new KrakenUserSpotDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<KrakenUserSpotDataTracker>>() ?? new NullLogger<KrakenUserSpotDataTracker>(),
                restClient,
                socketClient,
                userIdentifier,
                config
                );
        }

        /// <inheritdoc />
        public IUserFuturesDataTracker CreateUserFuturesDataTracker(FuturesUserDataTrackerConfig config)
        {
            var restClient = _serviceProvider?.GetRequiredService<IKrakenRestClient>() ?? new KrakenRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<IKrakenSocketClient>() ?? new KrakenSocketClient();
            return new KrakenUserFuturesDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<KrakenUserFuturesDataTracker>>() ?? new NullLogger<KrakenUserFuturesDataTracker>(),
                restClient,
                socketClient,
                null,
                config
                );
        }

        /// <inheritdoc />
        public IUserFuturesDataTracker CreateUserFuturesDataTracker(string userIdentifier, FuturesUserDataTrackerConfig config, ApiCredentials credentials, KrakenEnvironment? environment = null)
        {
            var clientProvider = _serviceProvider?.GetRequiredService<IKrakenUserClientProvider>() ?? new KrakenUserClientProvider();
            var restClient = clientProvider.GetRestClient(userIdentifier, credentials, environment);
            var socketClient = clientProvider.GetSocketClient(userIdentifier, credentials, environment);
            return new KrakenUserFuturesDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<KrakenUserFuturesDataTracker>>() ?? new NullLogger<KrakenUserFuturesDataTracker>(),
                restClient,
                socketClient,
                userIdentifier,
                config
                );
        }
    }
}
