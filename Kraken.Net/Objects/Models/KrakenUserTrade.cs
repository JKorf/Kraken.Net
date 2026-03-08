using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// User trade info
    /// </summary>
    [SerializationModel]
    public record KrakenUserTrade
    {
        /// <summary>
        /// ["<c>ordertxid</c>"] Order id
        /// </summary>
        [JsonPropertyName("ordertxid")]
        public string OrderId { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>postxid</c>"] Pos id
        /// </summary>
        [JsonPropertyName("postxid")]
        public string PosId { get; set; } = string.Empty;

        /// <summary>
        /// Trade id
        /// </summary>
        [JsonIgnore]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>pair</c>"] Symbol
        /// </summary>
        [JsonPropertyName("pair")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp of trade
        /// </summary>
        [JsonPropertyName("time"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>type</c>"] Side
        /// </summary>
        [JsonPropertyName("type")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>ordertype</c>"] Order type
        /// </summary>
        [JsonPropertyName("ordertype")]
        public OrderType Type { get; set; }
        /// <summary>
        /// ["<c>price</c>"] Price of the trade
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>cost</c>"] Cost of the trade
        /// </summary>
        [JsonPropertyName("cost")]
        public decimal QuoteQuantity { get; set; }
        /// <summary>
        /// ["<c>fee</c>"] Fee paid for trade
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// ["<c>vol</c>"] Quantity of the trade
        /// </summary>
        [JsonPropertyName("vol")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>margin</c>"] Margin
        /// </summary>
        [JsonPropertyName("margin")]
        public decimal Margin { get; set; }

        /// <summary>
        /// ["<c>misc</c>"] Misc info
        /// </summary>
        [JsonPropertyName("misc")]
        public string Misc { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>posstatus</c>"] Position status
        /// </summary>
        [JsonPropertyName("posstatus")]
        public string PositionStatus { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>cprice</c>"] Closed average price
        /// </summary>
        [JsonPropertyName("cprice")]
        public decimal? ClosedAveragePrice { get; set; }
        /// <summary>
        /// ["<c>ccost</c>"] Closed cost
        /// </summary>
        [JsonPropertyName("ccost")]
        public decimal? ClosedCost { get; set; }
        /// <summary>
        /// ["<c>cfee</c>"] Closed fee
        /// </summary>
        [JsonPropertyName("cfee")]
        public decimal? ClosedFee { get; set; }
        /// <summary>
        /// ["<c>cvol</c>"] Closed quantity
        /// </summary>
        [JsonPropertyName("cvol")]
        public decimal? ClosedQuantity { get; set; }
        /// <summary>
        /// ["<c>cmargin</c>"] Closed margin
        /// </summary>
        [JsonPropertyName("cmargin")]
        public decimal? ClosedMargin { get; set; }
        /// <summary>
        /// ["<c>net</c>"] Closed net profit/loss
        /// </summary>
        [JsonPropertyName("net")]
        public decimal? ClosedProfitLoss { get; set; }
        /// <summary>
        /// ["<c>maker</c>"] True if trade was executed with user as maker
        /// </summary>
        [JsonPropertyName("maker")]
        public bool Maker { get; set; }
        /// <summary>
        /// ["<c>trades</c>"] Trade ids
        /// </summary>
        [JsonPropertyName("trades")]
        public string[] Trades { get; set; } = Array.Empty<string>();
    }

    /// <summary>
    /// Stream trade update
    /// </summary>
    [SerializationModel]
    public record KrakenStreamUserTrade: KrakenUserTrade
    {
        /// <summary>
        /// ["<c>sequenceNumber</c>"] The update sequence number
        /// </summary>
        [JsonPropertyName("sequenceNumber")]
        public int SequenceNumber { get; set; }
    }
}
