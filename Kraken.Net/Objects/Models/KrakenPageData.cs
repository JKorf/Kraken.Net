using CryptoExchange.Net.Converters.SystemTextJson;
namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Base page data
    /// </summary>
    [SerializationModel]
    public record KrakenPageData
    {
        /// <summary>
        /// Total number of records
        /// </summary>
        [JsonPropertyName("count")]
        public int Count { get; set; }
    }

    /// <summary>
    /// Open orders page
    /// </summary>
    [SerializationModel]
    public record OpenOrdersPage : KrakenPageData
    {
        /// <summary>
        /// Open orders
        /// </summary>
        [JsonPropertyName("open")]
        public Dictionary<string, KrakenOrder> Open { get; set; } = new Dictionary<string, KrakenOrder>();
    }

    /// <summary>
    /// Closed orders page
    /// </summary>
    [SerializationModel]
    public record KrakenClosedOrdersPage: KrakenPageData
    {
        /// <summary>
        /// Closed orders
        /// </summary>
        [JsonPropertyName("closed")]
        public Dictionary<string, KrakenOrder> Closed { get; set; } = new Dictionary<string, KrakenOrder>();
    }

    /// <summary>
    /// User trades page
    /// </summary>
    [SerializationModel]
    public record KrakenUserTradesPage : KrakenPageData
    {
        /// <summary>
        /// Trades
        /// </summary>
        [JsonPropertyName("trades")]
        public Dictionary<string, KrakenUserTrade> Trades { get; set; } = new Dictionary<string, KrakenUserTrade>();
    }

    /// <summary>
    /// Ledger page
    /// </summary>
    [SerializationModel]
    public record KrakenLedgerPage : KrakenPageData
    {
        /// <summary>
        /// Ledger entries
        /// </summary>
        [JsonPropertyName("ledger")]
        public Dictionary<string, KrakenLedgerEntry> Ledger { get; set; } = new Dictionary<string, KrakenLedgerEntry>();
    }
}
