using Kraken.Net.Clients.SpotApi;
using Kraken.Net.Converters;
using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Order info
    /// </summary>
    [SerializationModel]
    public record KrakenOrder
    {
        /// <summary>
        /// The id of the order
        /// </summary>
        [JsonIgnore]
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>refid</c>"] Reference id
        /// </summary>
        [JsonPropertyName("refid")]
        public string ReferenceId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>userref</c>"] Client reference id
        /// </summary>
        [JsonPropertyName("userref")]
        public uint? UserReference { get; set; }
        /// <summary>
        /// ["<c>cl_ord_id</c>"] Client reference id
        /// </summary>
        [JsonPropertyName("cl_ord_id")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// ["<c>status</c>"] Status of the order
        /// </summary>

        [JsonPropertyName("status")]
        public OrderStatus Status { get; set; }
        /// <summary>
        /// Open timestamp
        /// </summary>
        [JsonPropertyName("opentm"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Start timestamp
        /// </summary>
        [JsonPropertyName("starttm"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// Expire timestamp
        /// </summary>
        [JsonPropertyName("expiretm"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? ExpireTime { get; set; }
        /// <summary>
        /// Close timestamp
        /// </summary>
        [JsonPropertyName("closetm"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? CloseTime { get; set; }

        /// <summary>
        /// ["<c>descr</c>"] Order details
        /// </summary>
        [JsonPropertyName("descr")]
        [JsonConverter(typeof(KrakenOrderDescriptionConverter))]
        public KrakenOrderInfo OrderDetails { get; set; } = default!;
        /// <summary>
        /// ["<c>vol</c>"] Quantity of the order
        /// </summary>
        [JsonPropertyName("vol")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>vol_exec</c>"] Filled quantity
        /// </summary>
        [JsonPropertyName("vol_exec")]
        public decimal QuantityFilled { get; set; }
        /// <summary>
        /// ["<c>cost</c>"] Cost of the order
        /// </summary>
        [JsonPropertyName("cost")]
        public decimal QuoteQuantityFilled { get; set; }
        /// <summary>
        /// ["<c>fee</c>"] Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// ["<c>price</c>"] Average price of the order
        /// </summary>
        [JsonPropertyName("price")]
        public decimal AveragePrice { get; set; }
        [JsonPropertyName("avg_price")]
        [JsonInclude]
        internal decimal AveragePrice2 { get => AveragePrice; set => AveragePrice = value; }
        /// <summary>
        /// ["<c>stopPrice</c>"] Stop price
        /// </summary>
        [JsonPropertyName("stopPrice")]
        public decimal StopPrice { get; set; }
        [JsonPropertyName("stopprice")]
        [JsonInclude]
        internal decimal StopPrice2 { get => StopPrice; set => StopPrice = value; }
        /// <summary>
        /// ["<c>limitprice</c>"] Limit price
        /// </summary>
        [JsonPropertyName("limitprice")]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>misc</c>"] Miscellaneous info
        /// </summary>
        [JsonPropertyName("misc")]
        public string Misc { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>oflags</c>"] Order flags
        /// </summary>
        [JsonPropertyName("oflags")]
        public string Oflags { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>reason</c>"] Reason of failure
        /// </summary>
        [JsonPropertyName("reason")]
        public string Reason { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>margin</c>"] Indicates if the order is funded on margin.
        /// </summary>
        [JsonPropertyName("margin")]
        public bool? Margin { get; set; }
        /// <summary>
        /// ["<c>trades</c>"] Trade ids
        /// </summary>
        [JsonPropertyName("trades")]
        public string[] TradeIds { get; set; } = Array.Empty<string>();
    }

    /// <summary>
    /// Order details
    /// </summary>
    [SerializationModel]
    public record KrakenOrderInfo
    {
        /// <summary>
        /// ["<c>pair</c>"] The symbol of the order
        /// </summary>
        [JsonPropertyName("pair")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>type</c>"] Side of the order
        /// </summary>
        [JsonPropertyName("type")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>ordertype</c>"] Type of the order
        /// </summary>
        [JsonPropertyName("ordertype")]
        public OrderType Type { get; set; }
        /// <summary>
        /// Trailing stop order deviation unit
        /// </summary>
        [JsonIgnore]
        public TrailingStopDeviationUnit? TrailingStopDeviationUnit { get; set; }
        /// <summary>
        /// Trailing stop order sign
        /// </summary>
        [JsonIgnore]
        public TrailingStopSign? TrailingStopSign { get; set; }
        /// <summary>
        /// Price of the order
        /// </summary>
        [JsonIgnore]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>price2</c>"] Secondary price of the order (<see cref="KrakenRestClientSpotApiTrading.PlaceOrderAsync"/> for details)
        /// </summary>
        [JsonPropertyName("price2")]
        public decimal SecondaryPrice { get; set; }
        /// <summary>
        /// ["<c>leverage</c>"] Amount of leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public string Leverage { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>order</c>"] Order description
        /// </summary>
        [JsonPropertyName("order")]
        public string Order { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>close</c>"] Conditional close order description
        /// </summary>
        [JsonPropertyName("close")]
        public string Close { get; set; } = string.Empty;
    }

    /// <summary>
    /// Stream order update
    /// </summary>
    [SerializationModel]
    public record KrakenStreamOrder: KrakenOrder
    {
        /// <summary>
        /// ["<c>sequenceNumber</c>"] The update sequence number
        /// </summary>
        [JsonPropertyName("sequenceNumber")]
        public int SequenceNumber { get; set; }
    }
}
