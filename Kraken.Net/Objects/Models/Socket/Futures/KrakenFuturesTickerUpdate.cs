using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Ticker info
    /// </summary>
    public class KrakenFuturesTickerUpdate: KrakenFuturesUpdateMessage
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonProperty("time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Funding rate
        /// </summary>
        [JsonProperty("funding_rate")]
        public decimal? FundingRate { get; set; }
        /// <summary>
        /// The estimated next funding rate
        /// </summary>
        [JsonProperty("funding_rate_prediction")]
        public decimal? FundingRatePrediction { get; set; }
        /// <summary>
        /// The absolute funding rate relative to the spot price at the time of funding rate calculation
        /// </summary>
        [JsonProperty("relative_funding_rate")]
        public decimal? RelativeFundingRate { get; set; }
        /// <summary>
        /// The estimated next absolute funding rate relative to the current spot price
        /// </summary>
        [JsonProperty("relative_funding_rate_prediction")]
        public decimal? RelativeFundingRatePrediction { get; set; }
        /// <summary>
        /// Next funding rate in miliseconds
        /// </summary>
        [JsonProperty("next_funding_rate_time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime NextFundingRateTime { get; set; }
        /// <summary>
        /// The best current bid price
        /// </summary>
        [JsonProperty("bid")]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// The quantity of the current best bid
        /// </summary>
        [JsonProperty("bid_size")]
        public decimal BestBidQuantity { get; set; }
        /// <summary>
        /// The best current ask price
        /// </summary>
        [JsonProperty("ask")]
        public decimal BestAskPrice { get; set; }
        /// <summary>
        /// The quantity of the current best ask
        /// </summary>
        [JsonProperty("ask_size")]
        public decimal BestAskQuantity { get; set; }
        /// <summary>
        /// The sum of the sizes of all fills observed in the last 24 hours
        /// </summary>
        [JsonProperty("volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// Days until maturity
        /// </summary>
        [JsonProperty("dtm")]
        public int? DaysUntilMaturity { get; set; }
        /// <summary>
        /// Leverage
        /// </summary>
        public string Leverage { get; set; } = string.Empty;
        /// <summary>
        /// The real time index of the symbol
        /// </summary>
        public decimal Index { get; set; }
        /// <summary>
        /// The premium associated with the symbol
        /// </summary>
        public decimal Premium { get; set; }
        /// <summary>
        /// The premium associated with the symbol
        /// </summary>
        [JsonProperty("last")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// The 24h change in price
        /// </summary>
        [JsonProperty("change")]
        public decimal ChangePercentage24h { get; set; }
        /// <summary>
        /// True if the market is suspended, false otherwise
        /// </summary>
        public bool Suspended { get; set; }
        /// <summary>
        /// Currently can be perpetual, month or quarter. Other tags may be added without notice.
        /// </summary>
        public string Tag { get; set; } = string.Empty;
        /// <summary>
        /// The currency pair of the symbol
        /// </summary>
        public string Pair { get; set; } = string.Empty;
        /// <summary>
        /// The current open interest of the symbol
        /// </summary>
        public decimal OpenInterest { get; set; }
        /// <summary>
        /// The market price of the symbol
        /// </summary>
        public decimal MarkPrice { get; set; }
        /// <summary>
        /// The maturity time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? MaturityTime { get; set; }
        /// <summary>
        /// True if the market is in post-only, false otherwise
        /// </summary>
        [JsonProperty("post_only")]
        public bool PostOnly { get; set; }
        /// <summary>
        /// The same as volume except that, for multi-collateral futures, it is converted to the non-base currency
        /// </summary>
        public decimal VolumeQuote { get; set; }
    }
}
