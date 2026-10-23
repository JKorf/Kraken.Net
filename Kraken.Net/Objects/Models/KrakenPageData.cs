namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Base page data
    /// </summary>
    [SerializationModel]
    public record KrakenPageData
    {
        /// <summary>
        /// ["<c>count</c>"] Total number of records
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
        /// ["<c>open</c>"] Open orders
        /// </summary>
        [JsonPropertyName("open")]
        public Dictionary<string, KrakenOrder> Open { get; set; } = new Dictionary<string, KrakenOrder>();

        /// <summary>
        /// ["<c>cursor</c>"] Cursor
        /// </summary>
        [JsonPropertyName("cursor")]
        public KrakenCursor? Cursor { get; set; }
    }

    /// <summary>
    /// Closed orders page
    /// </summary>
    [SerializationModel]
    public record KrakenClosedOrdersPage: KrakenPageData
    {
        /// <summary>
        /// ["<c>closed</c>"] Closed orders
        /// </summary>
        [JsonPropertyName("closed")]
        public Dictionary<string, KrakenOrder> Closed { get; set; } = new Dictionary<string, KrakenOrder>();

        /// <summary>
        /// ["<c>cursor</c>"] Cursor
        /// </summary>
        [JsonPropertyName("cursor")]
        public KrakenCursor? Cursor { get; set; }
    }

    /// <summary>
    /// User trades page
    /// </summary>
    [SerializationModel]
    public record KrakenUserTradesPage : KrakenPageData
    {
        /// <summary>
        /// ["<c>trades</c>"] Trades
        /// </summary>
        [JsonPropertyName("trades")]
        public Dictionary<string, KrakenUserTrade> Trades { get; set; } = new Dictionary<string, KrakenUserTrade>();

        /// <summary>
        /// ["<c>cursor</c>"] Cursor
        /// </summary>
        [JsonPropertyName("cursor")]
        public KrakenCursor? Cursor { get; set; }
    }

    /// <summary>
    /// Ledger page
    /// </summary>
    [SerializationModel]
    public record KrakenLedgerPage : KrakenPageData
    {
        /// <summary>
        /// ["<c>ledger</c>"] Ledger entries
        /// </summary>
        [JsonPropertyName("ledger")]
        public Dictionary<string, KrakenLedgerEntry> Ledger { get; set; } = new Dictionary<string, KrakenLedgerEntry>();
    }

    /// <summary>
    /// Page cursor
    /// </summary>
    public record KrakenCursor
    {
        /// <summary>
        /// ["<c>next</c>"] Next page cursor
        /// </summary>
        [JsonPropertyName("next")]
        public string? Next { get; set; }
    }

}
