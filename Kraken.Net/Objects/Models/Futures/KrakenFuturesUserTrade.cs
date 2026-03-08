using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Futures
{
    [SerializationModel]
    internal record KrakenFuturesUserTradeResult : KrakenFuturesResult<KrakenFuturesUserTrade[]>
    {
        [JsonPropertyName("fills")]
        public override KrakenFuturesUserTrade[] Data { get; set; } = [];
    }

    /// <summary>
    /// User trade info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesUserTrade
    {
        /// <summary>
        /// ["<c>cliOrdId</c>"] Client order id
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
        /// ["<c>fillType</c>"] Type of trade
        /// </summary>
        [JsonPropertyName("fillType")]

        public TradeType Type { get; set; }
        /// <summary>
        /// ["<c>fill_id</c>"] Trade id
        /// </summary>
        [JsonPropertyName("fill_id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>order_id</c>"] Order id
        /// </summary>
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>price</c>"] Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>side</c>"] Side
        /// </summary>

        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>size</c>"] Quantity
        /// </summary>
        [JsonPropertyName("size")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
    }
}
