using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Order to place
    /// </summary>
    [SerializationModel]
    public record KrakenOrderRequest
    {
        /// <summary>
        /// User reference
        /// </summary>
        [JsonPropertyName("userref"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public uint? UserReference { get; set; }
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("cl_ord_id"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Order type
        /// </summary>
        [JsonPropertyName("ordertype")]
        public OrderType OrderType { get; set; }
        /// <summary>
        /// Order side
        /// </summary>
        [JsonPropertyName("type")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("volume"), JsonConverter(typeof(DecimalStringWriterConverter))]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Iceberg quantity
        /// </summary>
        [JsonPropertyName("displayvol"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault), JsonConverter(typeof(DecimalStringWriterConverter))]
        public decimal? IcebergQuanty { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("price"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault), JsonConverter(typeof(DecimalStringWriterConverter))]
        public decimal? Price { get; set; }
        /// <summary>
        /// Secondary price
        /// </summary>
        [JsonPropertyName("price2"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault), JsonConverter(typeof(DecimalStringWriterConverter))]
        public decimal? SecondaryPrice { get; set; }
        /// <summary>
        /// Trigger
        /// </summary>
        [JsonPropertyName("trigger"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Trigger? Trigger { get; set; }
        /// <summary>
        /// Leverage
        /// </summary>
        [JsonPropertyName("leverage"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault), JsonConverter(typeof(DecimalStringWriterConverter))]
        public decimal? Leverage { get; set; }
        /// <summary>
        /// Reduce only
        /// </summary>
        [JsonPropertyName("reduce_only"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? ReduceOnly { get; set; }
        /// <summary>
        /// Self trade prevention type
        /// </summary>
        [JsonPropertyName("stptype"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public SelfTradePreventionType? SelfTradePreventionType { get; set; }
        /// <summary>
        /// Order flags
        /// </summary>
        [JsonPropertyName("oflags"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public OrderFlags[]? Flags { get; set; }
        /// <summary>
        /// Time in force
        /// </summary>
        [JsonPropertyName("timeinforce"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TimeInForce? TimeInForce { get; set; }
        /// <summary>
        /// Start time
        /// </summary>
        [JsonPropertyName("starttm"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// Expire time
        /// </summary>
        [JsonPropertyName("expiretm"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? ExpireTime { get; set; }
    }
}
