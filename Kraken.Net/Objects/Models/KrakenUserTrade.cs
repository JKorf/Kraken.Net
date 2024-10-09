using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// User trade info
    /// </summary>
    public record KrakenUserTrade
    {
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("ordertxid")]
        public string OrderId { get; set; } = string.Empty;

        /// <summary>
        /// Pos id
        /// </summary>
        [JsonPropertyName("postxid")]
        public string PosId { get; set; } = string.Empty;

        /// <summary>
        /// Trade id
        /// </summary>
        [JsonIgnore]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("pair")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp of trade
        /// </summary>
        [JsonPropertyName("time"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Side
        /// </summary>
        [JsonPropertyName("type"), JsonConverter(typeof(EnumConverter))]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Order type
        /// </summary>
        [JsonPropertyName("ordertype"), JsonConverter(typeof(EnumConverter))]
        public OrderType Type { get; set; }
        /// <summary>
        /// Price of the trade
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Cost of the trade
        /// </summary>
        [JsonPropertyName("cost")]
        public decimal QuoteQuantity { get; set; }
        /// <summary>
        /// Fee paid for trade
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// Quantity of the trade
        /// </summary>
        [JsonPropertyName("vol")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Margin
        /// </summary>
        [JsonPropertyName("margin")]
        public decimal Margin { get; set; }

        /// <summary>
        /// Misc info
        /// </summary>
        [JsonPropertyName("misc")]
        public string Misc { get; set; } = string.Empty;

        /// <summary>
        /// Position status
        /// </summary>
        [JsonPropertyName("posstatus")]
        public string PositionStatus { get; set; } = string.Empty;
        /// <summary>
        /// Closed average price
        /// </summary>
        [JsonPropertyName("cprice")]
        public decimal? ClosedAveragePrice { get; set; }
        /// <summary>
        /// Closed cost
        /// </summary>
        [JsonPropertyName("ccost")]
        public decimal? ClosedCost { get; set; }
        /// <summary>
        /// Closed fee
        /// </summary>
        [JsonPropertyName("cfee")]
        public decimal? ClosedFee { get; set; }
        /// <summary>
        /// Closed quantity
        /// </summary>
        [JsonPropertyName("cvol")]
        public decimal? ClosedQuantity { get; set; }
        /// <summary>
        /// Closed margin
        /// </summary>
        [JsonPropertyName("cmargin")]
        public decimal? ClosedMargin { get; set; }
        /// <summary>
        /// Closed net profit/loss
        /// </summary>
        [JsonPropertyName("net")]
        public decimal? ClosedProfitLoss { get; set; }
        /// <summary>
        /// True if trade was executed with user as maker
        /// </summary>
        [JsonPropertyName("maker")]
        public bool Maker { get; set; }
        /// <summary>
        /// Trade ids
        /// </summary>
        [JsonPropertyName("trades")]
        public IEnumerable<string> Trades { get; set; } = Array.Empty<string>();
    }

    /// <summary>
    /// Stream trade update
    /// </summary>
    public record KrakenStreamUserTrade: KrakenUserTrade
    {
        /// <summary>
        /// The update sequence number
        /// </summary>
        [JsonPropertyName("sequenceNumber")]
        public int SequenceNumber { get; set; }
    }
}
