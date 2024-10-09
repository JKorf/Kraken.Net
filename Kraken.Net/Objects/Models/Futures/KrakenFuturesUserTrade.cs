using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Futures
{
    internal record KrakenFuturesUserTradeResult : KrakenFuturesResult<IEnumerable<KrakenFuturesUserTrade>>
    {
        [JsonPropertyName("fills")]
        public override IEnumerable<KrakenFuturesUserTrade> Data { get; set; } = new List<KrakenFuturesUserTrade>();
    }

    /// <summary>
    /// User trade info
    /// </summary>
    public record KrakenFuturesUserTrade
    {
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("cliOrdId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Time of the trade
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("fillTime")]
        public DateTime FillTime { get; set; }
        /// <summary>
        /// Type of trade
        /// </summary>
        [JsonPropertyName("fillType")]
        [JsonConverter(typeof(EnumConverter))]
        public TradeType Type { get; set; }
        /// <summary>
        /// Trade id
        /// </summary>
        [JsonPropertyName("fill_id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Side
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("size")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
    }
}
