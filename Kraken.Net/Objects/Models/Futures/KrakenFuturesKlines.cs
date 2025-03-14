using CryptoExchange.Net.Converters.SystemTextJson;
namespace Kraken.Net.Objects.Models.Futures
{
    /// <summary>
    /// Kline info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesKlines
    {
        /// <summary>
        /// True if there are more candles in the time range
        /// </summary>
        [JsonPropertyName("more_candles")]
        public bool MoreKlines { get; set; }
        /// <summary>
        /// Candles
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
        /// High price
        /// </summary>
        [JsonPropertyName("high")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// Low price
        /// </summary>
        [JsonPropertyName("low")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// Close price
        /// </summary>
        [JsonPropertyName("close")]
        public decimal ClosePrice { get; set; }
        /// <summary>
        /// Open price
        /// </summary>
        [JsonPropertyName("open")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// Volume
        /// </summary>
        [JsonPropertyName("volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
    }
}
