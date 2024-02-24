using CryptoExchange.Net.Converters;
using Kraken.Net.Objects.Sockets;
using Newtonsoft.Json;
using System;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Ticker info
    /// </summary>
    public class KrakenFuturesMiniTickerUpdate : KrakenFuturesEvent
    {
        /// <summary>
        /// The best current bid price
        /// </summary>
        [JsonProperty("bid")]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// The best current ask price
        /// </summary>
        [JsonProperty("ask")]
        public decimal BestAskPrice { get; set; }
        /// <summary>
        /// The sum of the sizes of all fills observed in the last 24 hours
        /// </summary>
        [JsonProperty("volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// The premium associated with the symbol
        /// </summary>
        public decimal Premium { get; set; }
        /// <summary>
        /// The 24h change in price
        /// </summary>
        [JsonProperty("change")]
        public decimal ChangePercentage24h { get; set; }
        /// <summary>
        /// Currently can be perpetual, month or quarter. Other tags may be added without notice.
        /// </summary>
        public string Tag { get; set; } = string.Empty;
        /// <summary>
        /// The currency pair of the symbol
        /// </summary>
        public string Pair { get; set; } = string.Empty;
        /// <summary>
        /// The market price of the symbol
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? MaturityTime { get; set; }
        /// <summary>
        /// The same as volume except that, for multi-collateral futures, it is converted to the non-base currency
        /// </summary>
        public decimal VolumeQuote { get; set; }
    }
}
