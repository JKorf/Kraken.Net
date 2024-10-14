using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// Kline/candlestick info
    /// </summary>
    public record KrakenKlineUpdate
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Open price
        /// </summary>
        [JsonPropertyName("open")]
        public decimal OpenPrice { get; set; }
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
        /// Number of trades
        /// </summary>
        [JsonPropertyName("trades")]
        public decimal Trades { get; set; }
        /// <summary>
        /// Volume
        /// </summary>
        [JsonPropertyName("volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// Volume weighted average price
        /// </summary>
        [JsonPropertyName("vwap")]
        public decimal Vwap { get; set; }
        /// <summary>
        /// Open timestamp
        /// </summary>
        [JsonPropertyName("interval_begin")]
        public DateTime OpenTime { get; set; }
        /// <summary>
        /// Interval
        /// </summary>
        [JsonPropertyName("interval")]
        public KlineInterval KlineInterval { get; set; }
    }
}
