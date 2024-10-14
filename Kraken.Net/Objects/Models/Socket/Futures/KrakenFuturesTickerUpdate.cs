using Kraken.Net.Objects.Sockets;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Ticker info
    /// </summary>
    public record KrakenFuturesTickerUpdate: KrakenFuturesEvent
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Funding rate
        /// </summary>
        [JsonPropertyName("funding_rate")]
        public decimal? FundingRate { get; set; }
        /// <summary>
        /// The estimated next funding rate
        /// </summary>
        [JsonPropertyName("funding_rate_prediction")]
        public decimal? FundingRatePrediction { get; set; }
        /// <summary>
        /// The absolute funding rate relative to the spot price at the time of funding rate calculation
        /// </summary>
        [JsonPropertyName("relative_funding_rate")]
        public decimal? RelativeFundingRate { get; set; }
        /// <summary>
        /// The estimated next absolute funding rate relative to the current spot price
        /// </summary>
        [JsonPropertyName("relative_funding_rate_prediction")]
        public decimal? RelativeFundingRatePrediction { get; set; }
        /// <summary>
        /// Next funding rate in miliseconds
        /// </summary>
        [JsonPropertyName("next_funding_rate_time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime NextFundingRateTime { get; set; }
        /// <summary>
        /// The best current bid price
        /// </summary>
        [JsonPropertyName("bid")]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// The quantity of the current best bid
        /// </summary>
        [JsonPropertyName("bid_size")]
        public decimal BestBidQuantity { get; set; }
        /// <summary>
        /// The best current ask price
        /// </summary>
        [JsonPropertyName("ask")]
        public decimal BestAskPrice { get; set; }
        /// <summary>
        /// The quantity of the current best ask
        /// </summary>
        [JsonPropertyName("ask_size")]
        public decimal BestAskQuantity { get; set; }
        /// <summary>
        /// The sum of the sizes of all fills observed in the last 24 hours
        /// </summary>
        [JsonPropertyName("volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// Days until maturity
        /// </summary>
        [JsonPropertyName("dtm")]
        public int? DaysUntilMaturity { get; set; }
        /// <summary>
        /// Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public string Leverage { get; set; } = string.Empty;
        /// <summary>
        /// The real time index of the symbol
        /// </summary>
        [JsonPropertyName("index")]
        public decimal Index { get; set; }
        /// <summary>
        /// The premium associated with the symbol
        /// </summary>
        [JsonPropertyName("permium")]
        public decimal Premium { get; set; }
        /// <summary>
        /// The premium associated with the symbol
        /// </summary>
        [JsonPropertyName("last")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// The 24h change in price
        /// </summary>
        [JsonPropertyName("change")]
        public decimal ChangePercentage24h { get; set; }
        /// <summary>
        /// True if the market is suspended, false otherwise
        /// </summary>
        [JsonPropertyName("suspended")]
        public bool Suspended { get; set; }
        /// <summary>
        /// Currently can be perpetual, month or quarter. Other tags may be added without notice.
        /// </summary>
        [JsonPropertyName("tag")]
        public string Tag { get; set; } = string.Empty;
        /// <summary>
        /// The currency pair of the symbol
        /// </summary>
        [JsonPropertyName("pair")]
        public string Pair { get; set; } = string.Empty;
        /// <summary>
        /// The current open interest of the symbol
        /// </summary>
        [JsonPropertyName("openInterest")]
        public decimal OpenInterest { get; set; }
        /// <summary>
        /// The market price of the symbol
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
        /// True if the market is in post-only, false otherwise
        /// </summary>
        [JsonPropertyName("post_only")]
        public bool PostOnly { get; set; }
        /// <summary>
        /// The same as volume except that, for multi-collateral futures, it is converted to the non-base currency
        /// </summary>
        [JsonPropertyName("volumeQuote")]
        public decimal VolumeQuote { get; set; }
    }
}
