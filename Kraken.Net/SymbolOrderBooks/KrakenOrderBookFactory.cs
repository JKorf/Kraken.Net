using CryptoExchange.Net.Interfaces;
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

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public KrakenOrderBookFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public ISymbolOrderBook CreateSpot(string symbol, Action<KrakenOrderBookOptions>? options = null)
            => new KrakenSpotSymbolOrderBook(symbol,
                                        options,
                                        _serviceProvider.GetRequiredService<ILogger<KrakenSpotSymbolOrderBook>>(),
                                        _serviceProvider.GetRequiredService<IKrakenSocketClient>());
    }
}
