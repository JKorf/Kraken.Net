using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.OrderBook;
using Kraken.Net.Interfaces;
using Kraken.Net.Interfaces.Clients;
using Kraken.Net.Objects.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

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

            Spot = new OrderBookFactory<KrakenOrderBookOptions>((symbol, options) => CreateSpot(symbol, options), (baseAsset, quoteAsset, options) => CreateSpot(baseAsset.ToUpperInvariant() + "/" + quoteAsset.ToUpperInvariant(), options));
            Futures = new OrderBookFactory<KrakenOrderBookOptions>((symbol, options) => CreateFutures(symbol, options), (baseAsset, quoteAsset, options) => CreateFutures(baseAsset.ToUpperInvariant() + quoteAsset.ToUpperInvariant(), options));
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
