using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Trackers.Klines;
using CryptoExchange.Net.Trackers.Trades;
using Kraken.Net.Interfaces;
using Kraken.Net.Interfaces.Clients;
using Microsoft.Extensions.DependencyInjection;

namespace Kraken.Net
{
    /// <inheritdoc />
    public class KrakenTrackerFactory : IKrakenTrackerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public KrakenTrackerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public IKlineTracker CreateKlineTracker(SharedSymbol symbol, SharedKlineInterval interval, int? limit = null, TimeSpan? period = null)
        {
            IKlineRestClient restClient = _serviceProvider.GetRequiredService<IKrakenRestClient>().SpotApi.SharedClient;
            IKlineSocketClient socketClient = _serviceProvider.GetRequiredService<IKrakenSocketClient>().SpotApi.SharedClient;

            return new KlineTracker(
                _serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(restClient.Exchange),
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
            IRecentTradeRestClient? restClient = null;
            ITradeSocketClient socketClient;
            if (symbol.TradingMode == TradingMode.Spot)
            {
                restClient = _serviceProvider.GetRequiredService<IKrakenRestClient>().SpotApi.SharedClient;
                socketClient = _serviceProvider.GetRequiredService<IKrakenSocketClient>().SpotApi.SharedClient;
            }
            else
            {
                restClient = _serviceProvider.GetRequiredService<IKrakenRestClient>().FuturesApi.SharedClient;
                socketClient = _serviceProvider.GetRequiredService<IKrakenSocketClient>().FuturesApi.SharedClient;
            }

            return new TradeTracker(
                _serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(restClient.Exchange),
                restClient,
                socketClient,
                symbol,
                limit,
                period
                );
        }
    }
}
