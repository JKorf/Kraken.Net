using CryptoExchange.Net.Objects.Options;
using System;

namespace Kraken.Net.Objects.Options
{
    /// <summary>
    /// Options for the Kraken SymbolOrderBook
    /// </summary>
    public class KrakenOrderBookOptions : OrderBookOptions
    {
        /// <summary>
        /// Default options for the Kraken SymbolOrderBook
        /// </summary>
        public static KrakenOrderBookOptions Default { get; set; } = new KrakenOrderBookOptions();

        /// <summary>
        /// The limit of entries in the order book
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// After how much time we should consider the connection dropped if no data is received for this time after the initial subscriptions
        /// </summary>
        public TimeSpan? InitialDataTimeout { get; set; }

        internal KrakenOrderBookOptions Copy()
        {
            var options = Copy<KrakenOrderBookOptions>();
            options.Limit = Limit;
            options.InitialDataTimeout = InitialDataTimeout;
            return options;
        }
    }
}
