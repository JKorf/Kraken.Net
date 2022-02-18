using System.Collections.Generic;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Base page data
    /// </summary>
    public class KrakenPageData
    {
        /// <summary>
        /// Total number of records
        /// </summary>
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
        public Dictionary<string, KrakenOrder> Open { get; set; } = new Dictionary<string, KrakenOrder>();
    }

    /// <summary>
    /// Closed orders page
    /// </summary>
    public class KrakenClosedOrdersPage: KrakenPageData
    {
        /// <summary>
        /// Closed orders
        /// </summary>
        public Dictionary<string, KrakenOrder> Closed { get; set; } = new Dictionary<string, KrakenOrder>();
    }

    /// <summary>
    /// User trades page
    /// </summary>
    public class KrakenUserTradesPage : KrakenPageData
    {
        /// <summary>
        /// Trades
        /// </summary>
        public Dictionary<string, KrakenUserTrade> Trades { get; set; } = new Dictionary<string, KrakenUserTrade>();
    }

    /// <summary>
    /// Ledger page
    /// </summary>
    public class KrakenLedgerPage : KrakenPageData
    {
        /// <summary>
        /// Ledger entries
        /// </summary>
        public Dictionary<string, KrakenLedgerEntry> Ledger { get; set; } = new Dictionary<string, KrakenLedgerEntry>();
    }
}
