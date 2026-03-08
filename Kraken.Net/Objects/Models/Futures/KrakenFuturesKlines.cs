namespace Kraken.Net.Objects.Models.Futures
{
    /// <summary>
    /// Kline info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesKlines
    {
        /// <summary>
        /// ["<c>more_candles</c>"] True if there are more candles in the time range
        /// </summary>
        [JsonPropertyName("more_candles")]
        public bool MoreKlines { get; set; }
        /// <summary>
        /// ["<c>candles</c>"] Candles
        /// </summary>
        [JsonPropertyName("candles")]
        public KrakenFuturesKline[] Klines { get; set; } = Array.Empty<KrakenFuturesKline>();
    }

    /// <summary>
    /// Kline info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesKline
    {
        /// <summary>
        /// ["<c>high</c>"] High price
        /// </summary>
        [JsonPropertyName("high")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// ["<c>low</c>"] Low price
        /// </summary>
        [JsonPropertyName("low")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// ["<c>close</c>"] Close price
        /// </summary>
        [JsonPropertyName("close")]
        public decimal ClosePrice { get; set; }
        /// <summary>
        /// ["<c>open</c>"] Open price
        /// </summary>
        [JsonPropertyName("open")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// ["<c>volume</c>"] Volume
        /// </summary>
        [JsonPropertyName("volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// ["<c>time</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
    }
}
