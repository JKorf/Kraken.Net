using Kraken.Net.Objects.Options;

namespace Kraken.Net.Interfaces
{
    /// <summary>
    /// Kraken order book factory
    /// </summary>
    public interface IKrakenOrderBookFactory
    {
        /// <summary>
        /// Spot order book factory methods
        /// </summary>
        public IOrderBookFactory<KrakenOrderBookOptions> Spot { get; }

        /// <summary>
        /// Futures order book factory methods
        /// </summary>
        public IOrderBookFactory<KrakenOrderBookOptions> Futures { get; }

        /// <summary>
        /// Create a SymbolOrderBook for the Spot API
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="options">Order book options</param>
        /// <returns></returns>
        ISymbolOrderBook CreateSpot(string symbol, Action<KrakenOrderBookOptions>? options = null);

        /// <summary>
        /// Create a SymbolOrderBook for the Futures API
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="options">Order book options</param>
        /// <returns></returns>
        ISymbolOrderBook CreateFutures(string symbol, Action<KrakenOrderBookOptions>? options = null);
    }
}