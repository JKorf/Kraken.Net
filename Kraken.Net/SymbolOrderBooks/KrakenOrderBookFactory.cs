using CryptoExchange.Net.OrderBook;
using CryptoExchange.Net.SharedApis;
using Kraken.Net.Interfaces;
using Kraken.Net.Interfaces.Clients;
using Kraken.Net.Objects.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Kraken.Net.SymbolOrderBooks
{
    /// <inheritdoc />
    public class KrakenOrderBookFactory : IKrakenOrderBookFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <inheritdoc />
        public IOrderBookFactory<KrakenOrderBookOptions> Spot { get; }

        /// <inheritdoc />
        public IOrderBookFactory<KrakenOrderBookOptions> Futures { get; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public KrakenOrderBookFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            Spot = new OrderBookFactory<KrakenOrderBookOptions>(CreateSpot, Create);
            Futures = new OrderBookFactory<KrakenOrderBookOptions>(CreateFutures, Create);
        }

        /// <inheritdoc />
        public ISymbolOrderBook Create(SharedSymbol symbol, Action<KrakenOrderBookOptions>? options = null)
        {
            if (symbol.TradingMode == TradingMode.Spot)
                return CreateSpot(symbol.BaseAsset + "/" + symbol.QuoteAsset, options);

            var symbolName = symbol.GetSymbol(KrakenExchange.FormatSymbol);
            return CreateFutures(symbolName, options);
        }

        /// <inheritdoc />
        public ISymbolOrderBook CreateSpot(string symbol, Action<KrakenOrderBookOptions>? options = null)
            => new KrakenSpotSymbolOrderBook(symbol,
                                        options,
                                        _serviceProvider.GetRequiredService<ILoggerFactory>(),
                                        _serviceProvider.GetRequiredService<IKrakenSocketClient>());

        /// <inheritdoc />
        public ISymbolOrderBook CreateFutures(string symbol, Action<KrakenOrderBookOptions>? options = null)
            => new KrakenFuturesSymbolOrderBook(symbol,
                                        options,
                                        _serviceProvider.GetRequiredService<ILoggerFactory>(),
                                        _serviceProvider.GetRequiredService<IKrakenSocketClient>());
    }
}
