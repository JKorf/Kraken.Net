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
        /// ["<c>assets</c>"] Assets
        /// </summary>
        [JsonPropertyName("assets")]
        public KrakenSymbolUpdateAsset[] Assets { get; set; } = Array.Empty<KrakenSymbolUpdateAsset>();
        /// <summary>
        /// ["<c>pairs</c>"] Symbols
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
        /// ["<c>id</c>"] Asset
        /// </summary>
        [JsonPropertyName("id")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>status</c>"] Status
        /// </summary>
        [JsonPropertyName("status")]
        public AssetStatus AssetStatus { get; set; }
        /// <summary>
        /// ["<c>precision</c>"] Precision
        /// </summary>
        [JsonPropertyName("precision")]
        public decimal Precision { get; set; }
        /// <summary>
        /// ["<c>precision_display</c>"] Recommended display precision
        /// </summary>
        [JsonPropertyName("precision_display")]
        public decimal PrecisionDisplay { get; set; }
        /// <summary>
        /// ["<c>borrowable</c>"] Borrowable
        /// </summary>
        [JsonPropertyName("borrowable")]
        public bool Borrowable { get; set; }
        /// <summary>
        /// ["<c>collateral_value</c>"] Collateral value
        /// </summary>
        [JsonPropertyName("collateral_value")]
        public decimal CollateralValue { get; set; }
        /// <summary>
        /// ["<c>margin_rate</c>"] Margin rate
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
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>base</c>"] Base asset
        /// </summary>
        [JsonPropertyName("base")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>quote</c>"] Quote asset
        /// </summary>
        [JsonPropertyName("quote")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>status</c>"] Status
        /// </summary>
        [JsonPropertyName("status")]
        public SymbolStatus SymbolStatus { get; set; }
        /// <summary>
        /// ["<c>qty_precision</c>"] Quantity precision
        /// </summary>
        [JsonPropertyName("qty_precision")]
        public decimal QuantityPrecision { get; set; }
        /// <summary>
        /// ["<c>qty_increment</c>"] Quantity increment step
        /// </summary>
        [JsonPropertyName("qty_increment")]
        public decimal QuantityStep { get; set; }
        /// <summary>
        /// ["<c>price_precision</c>"] Price precision
        /// </summary>
        [JsonPropertyName("price_precision")]
        public decimal PricePrecision { get; set; }
        /// <summary>
        /// ["<c>cost_precision</c>"] Cost precision
        /// </summary>
        [JsonPropertyName("cost_precision")]
        public decimal CostPrecision { get; set; }
        /// <summary>
        /// ["<c>marginable</c>"] Marginable
        /// </summary>
        [JsonPropertyName("marginable")]
        public bool Marginable { get; set; }
        /// <summary>
        /// ["<c>has_index</c>"] Has index
        /// </summary>
        [JsonPropertyName("has_index")]
        public bool HasIndex { get; set; }
        /// <summary>
        /// ["<c>cost_min</c>"] Minimal notional value of an order
        /// </summary>
        [JsonPropertyName("cost_min")]
        public decimal MinNotionalValue { get; set; }
        /// <summary>
        /// ["<c>margin_initial</c>"] Initial margin requirement
        /// </summary>
        [JsonPropertyName("margin_initial")]
        public decimal MarginInitial { get; set; }
        /// <summary>
        /// ["<c>position_limit_long</c>"] Position limit long
        /// </summary>
        [JsonPropertyName("position_limit_long")]
        public decimal PositionLimitLong { get; set; }
        /// <summary>
        /// ["<c>position_limit_short</c>"] Position limit short
        /// </summary>
        [JsonPropertyName("position_limit_short")]
        public decimal PositionLimitShort { get; set; }
        /// <summary>
        /// ["<c>price_increment</c>"] Price increment step
        /// </summary>
        [JsonPropertyName("price_increment")]
        public decimal PriceStep { get; set; }
        /// <summary>
        /// ["<c>qty_min</c>"] Min order quantity
        /// </summary>
        [JsonPropertyName("qty_min")]
        public decimal MinOrderQuantity { get; set; }
    }


}
