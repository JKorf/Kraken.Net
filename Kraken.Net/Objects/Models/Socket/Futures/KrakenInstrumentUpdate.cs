using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Symbol and asset updates
    /// </summary>
    [SerializationModel]
    public record KrakenInstrumentUpdate
    {
        /// <summary>
        /// Assets
        /// </summary>
        [JsonPropertyName("assets")]
        public KrakenSymbolUpdateAsset[] Assets { get; set; } = Array.Empty<KrakenSymbolUpdateAsset>();
        /// <summary>
        /// Symbols
        /// </summary>
        [JsonPropertyName("pairs")]
        public KrakenSymbolUpdateSymbol[] Symbols { get; set; } = Array.Empty<KrakenSymbolUpdateSymbol>();
    }

    /// <summary>
    /// Asset info
    /// </summary>
    [SerializationModel]
    public record KrakenSymbolUpdateAsset
    {
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("id")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("status")]
        public AssetStatus AssetStatus { get; set; }
        /// <summary>
        /// Precision
        /// </summary>
        [JsonPropertyName("precision")]
        public decimal Precision { get; set; }
        /// <summary>
        /// Recommended display precision
        /// </summary>
        [JsonPropertyName("precision_display")]
        public decimal PrecisionDisplay { get; set; }
        /// <summary>
        /// Borrowable
        /// </summary>
        [JsonPropertyName("borrowable")]
        public bool Borrowable { get; set; }
        /// <summary>
        /// Collateral value
        /// </summary>
        [JsonPropertyName("collateral_value")]
        public decimal CollateralValue { get; set; }
        /// <summary>
        /// Margin rate
        /// </summary>
        [JsonPropertyName("margin_rate")]
        public decimal MarginRate { get; set; }
    }

    /// <summary>
    /// Symbol info
    /// </summary>
    [SerializationModel]
    public record KrakenSymbolUpdateSymbol
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Base asset
        /// </summary>
        [JsonPropertyName("base")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// Quote asset
        /// </summary>
        [JsonPropertyName("quote")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("status")]
        public SymbolStatus SymbolStatus { get; set; }
        /// <summary>
        /// Quantity precision
        /// </summary>
        [JsonPropertyName("qty_precision")]
        public decimal QuantityPrecision { get; set; }
        /// <summary>
        /// Quantity increment step
        /// </summary>
        [JsonPropertyName("qty_increment")]
        public decimal QuantityStep { get; set; }
        /// <summary>
        /// Price precision
        /// </summary>
        [JsonPropertyName("price_precision")]
        public decimal PricePrecision { get; set; }
        /// <summary>
        /// Cost precision
        /// </summary>
        [JsonPropertyName("cost_precision")]
        public decimal CostPrecision { get; set; }
        /// <summary>
        /// Marginable
        /// </summary>
        [JsonPropertyName("marginable")]
        public bool Marginable { get; set; }
        /// <summary>
        /// Has index
        /// </summary>
        [JsonPropertyName("has_index")]
        public bool HasIndex { get; set; }
        /// <summary>
        /// Minimal notional value of an order
        /// </summary>
        [JsonPropertyName("cost_min")]
        public decimal MinNotionalValue { get; set; }
        /// <summary>
        /// Initial margin requirement
        /// </summary>
        [JsonPropertyName("margin_initial")]
        public decimal MarginInitial { get; set; }
        /// <summary>
        /// Position limit long
        /// </summary>
        [JsonPropertyName("position_limit_long")]
        public decimal PositionLimitLong { get; set; }
        /// <summary>
        /// Position limit short
        /// </summary>
        [JsonPropertyName("position_limit_short")]
        public decimal PositionLimitShort { get; set; }
        /// <summary>
        /// Price increment step
        /// </summary>
        [JsonPropertyName("price_increment")]
        public decimal PriceStep { get; set; }
        /// <summary>
        /// Min order quantity
        /// </summary>
        [JsonPropertyName("qty_min")]
        public decimal MinOrderQuantity { get; set; }
    }


}
