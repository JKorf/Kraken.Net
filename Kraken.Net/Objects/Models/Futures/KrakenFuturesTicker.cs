namespace Kraken.Net.Objects.Models.Futures
{
    [SerializationModel]
    internal record KrakenFuturesTickersResult : KrakenFuturesResult<KrakenFuturesTicker[]>
    {
        [JsonPropertyName("tickers")]
        public override KrakenFuturesTicker[] Data { get; set; } = Array.Empty<KrakenFuturesTicker>();
    }

    [SerializationModel]
    internal record KrakenFuturesTickerResult : KrakenFuturesResult<KrakenFuturesTicker>
    {
        [JsonPropertyName("ticker")]
        public override KrakenFuturesTicker Data { get; set; } = null!;
    }

    /// <summary>
    /// Ticker info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesTicker
    {
        /// <summary>
        /// ["<c>ask</c>"] The price of the current best ask
        /// </summary>
        [JsonPropertyName("ask")]
        public decimal? BestAskPrice { get; set; }
        /// <summary>
        /// ["<c>askSize</c>"] The size of the current best ask
        /// </summary>
        [JsonPropertyName("askSize")]
        public decimal? BestAskQuantity { get; set; }
        /// <summary>
        /// ["<c>bid</c>"] The price of the current best bid
        /// </summary>
        [JsonPropertyName("bid")]
        public decimal? BestBidPrice { get; set; }
        /// <summary>
        /// ["<c>bidSize</c>"] The size of the current best bid
        /// </summary>
        [JsonPropertyName("bidSize")]
        public decimal? BestBidQuantity { get; set; }
        /// <summary>
        /// ["<c>fundingRate</c>"] The current absolute funding rate.
        /// </summary>
        [JsonPropertyName("fundingRate")]
        public decimal? FundingRate { get; set; }
        /// <summary>
        /// ["<c>fundingRatePrediction</c>"] The estimated next absolute funding rate.
        /// </summary>
        [JsonPropertyName("fundingRatePrediction")]
        public decimal? FundingRatePrediction { get; set; }
        /// <summary>
        /// ["<c>indexPrice</c>"] Index price
        /// </summary>
        [JsonPropertyName("indexPrice")]
        public decimal? IndexPrice { get; set; }
        /// <summary>
        /// ["<c>last</c>"] For futures: The price of the last fill. For indices: The last calculated value
        /// </summary>
        [JsonPropertyName("last")]
        public decimal? LastPrice { get; set; }
        /// <summary>
        /// ["<c>lastSize</c>"] The size of the last fill
        /// </summary>
        [JsonPropertyName("lastSize")]
        public decimal? LastQuantity { get; set; }
        /// <summary>
        /// The date and time at which last price was observed.
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("lastTime")]
        public DateTime? LastTradeTime { get; set; }
        /// <summary>
        /// ["<c>markPrice</c>"] The price to which Kraken Futures currently marks the Futures for margining purposes
        /// </summary>
        [JsonPropertyName("markPrice")]
        public decimal MarkPrice { get; set; }
        /// <summary>
        /// ["<c>open24h</c>"] The price of the fill observed 24 hours ago
        /// </summary>
        [JsonPropertyName("open24h")]
        public decimal? OpenPrice24h { get; set; }
        /// <summary>
        /// ["<c>openInterest</c>"] The current open interest of the symbol
        /// </summary>
        [JsonPropertyName("openInterest")]
        public decimal OpenInterest { get; set; }
        /// <summary>
        /// ["<c>pair</c>"] The currency pair of the symbol
        /// </summary>
        [JsonPropertyName("pair")]
        public string Pair { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>postOnly</c>"] Post only
        /// </summary>
        [JsonPropertyName("postOnly")]
        public bool PostOnly { get; set; }
        /// <summary>
        /// ["<c>suspended</c>"] True if the market is suspended, False otherwise.
        /// </summary>
        [JsonPropertyName("suspended")]
        public bool Suspended { get; set; }
        /// <summary>
        /// ["<c>symbol</c>"] The symbol of the Futures.
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>tag</c>"] Currently can be 'perpetual', 'month' or 'quarter'. Other tags may be added without notice
        /// </summary>
        [JsonPropertyName("tag")]
        public string? Tag { get; set; }
        /// <summary>
        /// ["<c>vol24h</c>"] The sum of the sizes of all fills observed in the last 24 hours
        /// </summary>
        [JsonPropertyName("vol24h")]
        public decimal Volume24h { get; set; }
        /// <summary>
        /// ["<c>volumeQuote</c>"] The sum of the size * price of all fills observed in the last 24 hours
        /// </summary>
        [JsonPropertyName("volumeQuote")]
        public decimal Volume24hQuote { get; set; }
    }
}
