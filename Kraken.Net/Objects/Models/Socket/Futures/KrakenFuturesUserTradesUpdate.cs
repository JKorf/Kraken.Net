using CryptoExchange.Net.Converters.SystemTextJson;
using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// User trades update
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesUserTradesUpdate : KrakenFuturesSocketMessage
    {
        /// <summary>
        /// Account
        /// </summary>
        [JsonPropertyName("account")]
        public string Account { get; set; } = string.Empty;
        /// <summary>
        /// Trades
        /// </summary>
        [JsonPropertyName("fills")]
        public KrakenFuturesStreamUserTrade[] Trades { get; set; } = Array.Empty<KrakenFuturesStreamUserTrade>();
    }

    /// <summary>
    /// User trade update info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesStreamUserTrade
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("instrument")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("qty")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Sequence number
        /// </summary>
        [JsonPropertyName("seq")]
        public long Sequence { get; set; }
        /// <summary>
        /// Is buy
        /// </summary>
        [JsonPropertyName("buy")]
        public bool Buy { get; set; }
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("cli_ord_id")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Trade id
        /// </summary>
        [JsonPropertyName("fill_id")]
        public string TradeId { get; set; } = string.Empty;
        /// <summary>
        /// Trade type
        /// </summary>

        [JsonPropertyName("fill_type")]
        public TradeType TradeType { get; set; }
        /// <summary>
        /// Fee paid on trade
        /// </summary>
        [JsonPropertyName("fee_paid")]
        public decimal FeePaid { get; set; }
        /// <summary>
        /// Fee currency
        /// </summary>
        [JsonPropertyName("fee_currency")]
        public string FeeCurrency { get; set; } = string.Empty;
        /// <summary>
        /// Order type of the taker
        /// </summary>
        [JsonPropertyName("taker_order_type")]
        public string TakerOrderType { get; set; } = string.Empty;
        /// <summary>
        /// Order type
        /// </summary>
        [JsonPropertyName("order_type")]
        public string OrderType { get; set; } = string.Empty;
    }
}
