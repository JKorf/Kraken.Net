using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Futures
{
    internal class KrakenFuturesTickerResult : KrakenFuturesResult<IEnumerable<KrakenFuturesTicker>>
    {
        [JsonProperty("tickers")]
        public override IEnumerable<KrakenFuturesTicker> Data { get; set; } = Array.Empty<KrakenFuturesTicker>();
    }

    /// <summary>
    /// Ticker info
    /// </summary>
    public class KrakenFuturesTicker
    {
        /// <summary>
        /// The price of the current best ask
        /// </summary>
        [JsonProperty("ask")]
        public decimal? BestAskPrice { get; set; }
        /// <summary>
        /// The size of the current best ask
        /// </summary>
        [JsonProperty("askSize")]
        public decimal? BestAskQuantity { get; set; }
        /// <summary>
        /// The price of the current best bid
        /// </summary>
        [JsonProperty("bid")]
        public decimal? BestBidPrice { get; set; }
        /// <summary>
        /// The size of the current best bid
        /// </summary>
        [JsonProperty("bidSize")]
        public decimal? BestBidQuantity { get; set; }
        /// <summary>
        /// The current absolute funding rate.
        /// </summary>
        public decimal? FundingRate { get; set; }
        /// <summary>
        /// The estimated next absolute funding rate.
        /// </summary>
        public decimal? FundingRatePredication { get; set; }
        /// <summary>
        /// Index price
        /// </summary>
        public decimal? IndexPrice { get; set; }
        /// <summary>
        /// For futures: The price of the last fill. For indices: The last calculated value
        /// </summary>
        [JsonProperty("last")]
        public decimal? LastPrice { get; set; }
        /// <summary>
        /// The size of the last fill
        /// </summary>
        [JsonProperty("lastSize")]
        public decimal? LastQuantity { get; set; }
        /// <summary>
        /// The date and time at which last price was observed.
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("lastTime")]
        public DateTime? LastTradeTime { get; set; }
        /// <summary>
        /// The price to which Kraken Futures currently marks the Futures for margining purposes
        /// </summary>
        public decimal MarkPrice { get; set; }
        /// <summary>
        /// The price of the fill observed 24 hours ago
        /// </summary>
        [JsonProperty("open24h")]
        public decimal? OpenPrice24h { get; set; }
        /// <summary>
        /// The current open interest of the symbol
        /// </summary>
        public decimal OpenInterest { get; set; }
        /// <summary>
        /// The currency pair of the symbol
        /// </summary>
        public string Pair { get; set; } = string.Empty;
        /// <summary>
        /// Post only
        /// </summary>
        public bool PostOnly { get; set; }
        /// <summary>
        /// True if the market is suspended, False otherwise.
        /// </summary>
        public bool Suspended { get; set; }
        /// <summary>
        /// The symbol of the Futures.
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Currently can be 'perpetual', 'month' or 'quarter'. Other tags may be added without notice
        /// </summary>
        public string? Tag { get; set; }
        /// <summary>
        /// The sum of the sizes of all fills observed in the last 24 hours
        /// </summary>
        [JsonProperty("vol24h")]
        public decimal Volume24h { get; set; }
        /// <summary>
        /// The sum of the size * price of all fills observed in the last 24 hours
        /// </summary>
        [JsonProperty("volumeQuote")]
        public decimal Volume24hQuote { get; set; }
    }
}
