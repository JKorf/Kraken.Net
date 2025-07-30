using CryptoExchange.Net.Converters.SystemTextJson;
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
        /// Reference id
        /// </summary>
        [JsonPropertyName("refid")]
        public string ReferenceId { get; set; } = string.Empty;
        /// <summary>
        /// Client reference id
        /// </summary>
        [JsonPropertyName("userref")]
        public uint? UserReference { get; set; }
        /// <summary>
        /// Client reference id
        /// </summary>
        [JsonPropertyName("cl_ord_id")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Status of the order
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
        /// Order details
        /// </summary>
        [JsonPropertyName("descr")]
        [JsonConverter(typeof(KrakenOrderDescriptionConverter))]
        public KrakenOrderInfo OrderDetails { get; set; } = default!;
        /// <summary>
        /// Quantity of the order
        /// </summary>
        [JsonPropertyName("vol")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Filled quantity
        /// </summary>
        [JsonPropertyName("vol_exec")]
        public decimal QuantityFilled { get; set; }
        /// <summary>
        /// Cost of the order
        /// </summary>
        [JsonPropertyName("cost")]
        public decimal QuoteQuantityFilled { get; set; }
        /// <summary>
        /// Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// Average price of the order
        /// </summary>
        [JsonPropertyName("price")]
        public decimal AveragePrice { get; set; }
        [JsonPropertyName("avg_price")]
        [JsonInclude]
        internal decimal AveragePrice2 { get => AveragePrice; set => AveragePrice = value; }
        /// <summary>
        /// Stop price
        /// </summary>
        [JsonPropertyName("stopPrice")]
        public decimal StopPrice { get; set; }
        [JsonPropertyName("stopprice")]
        [JsonInclude]
        internal decimal StopPrice2 { get => StopPrice; set => StopPrice = value; }
        /// <summary>
        /// Limit price
        /// </summary>
        [JsonPropertyName("limitprice")]
        public decimal Price { get; set; }
        /// <summary>
        /// Miscellaneous info
        /// </summary>
        [JsonPropertyName("misc")]
        public string Misc { get; set; } = string.Empty;
        /// <summary>
        /// Order flags
        /// </summary>
        [JsonPropertyName("oflags")]
        public string Oflags { get; set; } = string.Empty;
        /// <summary>
        /// Reason of failure
        /// </summary>
        [JsonPropertyName("reason")]
        public string Reason { get; set; } = string.Empty;
        /// <summary>
        /// Indicates if the order is funded on margin.
        /// </summary>
        [JsonPropertyName("margin")]
        public bool? Margin { get; set; }
        /// <summary>
        /// Trade ids
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
        /// The symbol of the order
        /// </summary>
        [JsonPropertyName("pair")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Side of the order
        /// </summary>
        [JsonPropertyName("type")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Type of the order
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
        /// Secondary price of the order (<see cref="KrakenRestClientSpotApiTrading.PlaceOrderAsync"/> for details)
        /// </summary>
        [JsonPropertyName("price2")]
        public decimal SecondaryPrice { get; set; }
        /// <summary>
        /// Amount of leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public string Leverage { get; set; } = string.Empty;
        /// <summary>
        /// Order description
        /// </summary>
        [JsonPropertyName("order")]
        public string Order { get; set; } = string.Empty;
        /// <summary>
        /// Conditional close order description
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
        /// The update sequence number
        /// </summary>
        [JsonPropertyName("sequenceNumber")]
        public int SequenceNumber { get; set; }
    }
}
