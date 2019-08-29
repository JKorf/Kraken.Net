using System.Collections.Generic;
using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Objects
{
    /// <summary>
    /// Base page data
    /// </summary>
    public class KrakenPageData
    {
        /// <summary>
        /// Total number of records
        /// </summary>
        [JsonOptionalProperty]
        public int Count { get; set; }
    }

    /// <summary>
    /// Open orders page
    /// </summary>
    public class OpenOrdersPage : KrakenPageData
    {
        /// <summary>
        /// Open orders
        /// </summary>
        public Dictionary<string, KrakenOrder> Open { get; set; }
    }

    /// <summary>
    /// Closed orders page
    /// </summary>
    public class KrakenClosedOrdersPage: KrakenPageData
    {
        /// <summary>
        /// Closed orders
        /// </summary>
        public Dictionary<string, KrakenOrder> Closed { get; set; }
    }

    /// <summary>
    /// User trades page
    /// </summary>
    public class KrakenUserTradesPage : KrakenPageData
    {
        /// <summary>
        /// Trades
        /// </summary>
        public Dictionary<string, KrakenUserTrade> Trades { get; set; }
    }

    /// <summary>
    /// Ledger page
    /// </summary>
    public class KrakenLedgerPage : KrakenPageData
    {
        /// <summary>
        /// Ledger entries
        /// </summary>
        public Dictionary<string, KrakenLedgerEntry> Ledger { get; set; }
    }
}
