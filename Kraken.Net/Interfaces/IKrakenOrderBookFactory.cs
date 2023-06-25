using CryptoExchange.Net.Interfaces;
using Kraken.Net.Objects.Options;
using System;

namespace Kraken.Net.Interfaces
{
    /// <summary>
    /// Kraken order book factory
    /// </summary>
    public interface IKrakenOrderBookFactory
    {
        /// <summary>
        /// Create a SymbolOrderBook for the Spot API
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="options">Order book options</param>
        /// <returns></returns>
        ISymbolOrderBook CreateSpot(string symbol, Action<KrakenOrderBookOptions>? options = null);
    }
}