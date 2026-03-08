using Kraken.Net.Objects.Sockets;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Ticker info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesMiniTickerUpdate : KrakenFuturesEvent
    {
        /// <summary>
        /// ["<c>bid</c>"] The best current bid price
        /// </summary>
        [JsonPropertyName("bid")]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// ["<c>ask</c>"] The best current ask price
        /// </summary>
        [JsonPropertyName("ask")]
        public decimal BestAskPrice { get; set; }
        /// <summary>
        /// ["<c>volume</c>"] The sum of the sizes of all fills observed in the last 24 hours
        /// </summary>
        [JsonPropertyName("volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// ["<c>premium</c>"] The premium associated with the symbol
        /// </summary>
        [JsonPropertyName("premium")]
        public decimal Premium { get; set; }
        /// <summary>
        /// ["<c>change</c>"] The 24h change in price
        /// </summary>
        [JsonPropertyName("change")]
        public decimal ChangePercentage24h { get; set; }
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
        /// The market price of the symbol
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("maturityTime")]
        public DateTime? MaturityTime { get; set; }
        /// <summary>
        /// ["<c>volumeQuote</c>"] The same as volume except that, for multi-collateral futures, it is converted to the non-base currency
        /// </summary>
        [JsonPropertyName("volumeQuote")]
        public decimal VolumeQuote { get; set; }
    }
}
