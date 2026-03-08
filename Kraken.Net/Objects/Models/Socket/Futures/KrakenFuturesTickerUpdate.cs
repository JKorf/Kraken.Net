using Kraken.Net.Objects.Sockets;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Ticker info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesTickerUpdate: KrakenFuturesEvent
    {
        /// <summary>
        /// ["<c>time</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>funding_rate</c>"] Funding rate
        /// </summary>
        [JsonPropertyName("funding_rate")]
        public decimal? FundingRate { get; set; }
        /// <summary>
        /// ["<c>funding_rate_prediction</c>"] The estimated next funding rate
        /// </summary>
        [JsonPropertyName("funding_rate_prediction")]
        public decimal? FundingRatePrediction { get; set; }
        /// <summary>
        /// ["<c>relative_funding_rate</c>"] The absolute funding rate relative to the spot price at the time of funding rate calculation
        /// </summary>
        [JsonPropertyName("relative_funding_rate")]
        public decimal? RelativeFundingRate { get; set; }
        /// <summary>
        /// ["<c>relative_funding_rate_prediction</c>"] The estimated next absolute funding rate relative to the current spot price
        /// </summary>
        [JsonPropertyName("relative_funding_rate_prediction")]
        public decimal? RelativeFundingRatePrediction { get; set; }
        /// <summary>
        /// ["<c>next_funding_rate_time</c>"] Next funding rate in miliseconds
        /// </summary>
        [JsonPropertyName("next_funding_rate_time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime NextFundingRateTime { get; set; }
        /// <summary>
        /// ["<c>bid</c>"] The best current bid price
        /// </summary>
        [JsonPropertyName("bid")]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// ["<c>bid_size</c>"] The quantity of the current best bid
        /// </summary>
        [JsonPropertyName("bid_size")]
        public decimal BestBidQuantity { get; set; }
        /// <summary>
        /// ["<c>ask</c>"] The best current ask price
        /// </summary>
        [JsonPropertyName("ask")]
        public decimal BestAskPrice { get; set; }
        /// <summary>
        /// ["<c>ask_size</c>"] The quantity of the current best ask
        /// </summary>
        [JsonPropertyName("ask_size")]
        public decimal BestAskQuantity { get; set; }
        /// <summary>
        /// ["<c>volume</c>"] The sum of the sizes of all fills observed in the last 24 hours
        /// </summary>
        [JsonPropertyName("volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// ["<c>dtm</c>"] Days until maturity
        /// </summary>
        [JsonPropertyName("dtm")]
        public int? DaysUntilMaturity { get; set; }
        /// <summary>
        /// ["<c>leverage</c>"] Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public string Leverage { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>index</c>"] The real time index of the symbol
        /// </summary>
        [JsonPropertyName("index")]
        public decimal Index { get; set; }
        /// <summary>
        /// ["<c>permium</c>"] The premium associated with the symbol
        /// </summary>
        [JsonPropertyName("permium")]
        public decimal Premium { get; set; }
        /// <summary>
        /// ["<c>last</c>"] The premium associated with the symbol
        /// </summary>
        [JsonPropertyName("last")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// ["<c>change</c>"] The 24h change in price
        /// </summary>
        [JsonPropertyName("change")]
        public decimal ChangePercentage24h { get; set; }
        /// <summary>
        /// ["<c>suspended</c>"] True if the market is suspended, false otherwise
        /// </summary>
        [JsonPropertyName("suspended")]
        public bool Suspended { get; set; }
        /// <summary>
        /// ["<c>tag</c>"] Currently can be perpetual, month or quarter. Other tags may be added without notice.
        /// </summary>
        [JsonPropertyName("tag")]
        public string Tag { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>pair</c>"] The currency pair of the symbol
        /// </summary>
        [JsonPropertyName("pair")]
        public string Pair { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>openInterest</c>"] The current open interest of the symbol
        /// </summary>
        [JsonPropertyName("openInterest")]
        public decimal OpenInterest { get; set; }
        /// <summary>
        /// ["<c>markPrice</c>"] The market price of the symbol
        /// </summary>
        [JsonPropertyName("markPrice")]
        public decimal MarkPrice { get; set; }
        /// <summary>
        /// The maturity time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("maturityTime")]
        public DateTime? MaturityTime { get; set; }
        /// <summary>
        /// ["<c>post_only</c>"] True if the market is in post-only, false otherwise
        /// </summary>
        [JsonPropertyName("post_only")]
        public bool PostOnly { get; set; }
        /// <summary>
        /// ["<c>volumeQuote</c>"] The same as volume except that, for multi-collateral futures, it is converted to the non-base currency
        /// </summary>
        [JsonPropertyName("volumeQuote")]
        public decimal VolumeQuote { get; set; }
    }
}
