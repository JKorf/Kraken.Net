using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Symbol info
    /// </summary>
    [SerializationModel]
    public record KrakenSymbol
    {
        /// <summary>
        /// ["<c>altname</c>"] Alternative name
        /// </summary>
        [JsonPropertyName("altname")]
        public string AlternateName { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>wsname</c>"] Name to use for the socket client subscriptions
        /// </summary>
        [JsonPropertyName("wsname")]
        public string WebsocketName { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>aclass_base</c>"] Class of the base asset
        /// </summary>
        [JsonPropertyName("aclass_base")]
        public string BaseAssetClass { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>base</c>"] Name of the base asset
        /// </summary>
        [JsonPropertyName("base")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>aclass_quote</c>"] Class of the quote asset
        /// </summary>
        [JsonPropertyName("aclass_quote")]
        public string QuoteAssetClass { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>quote</c>"] Name of the quote asset
        /// </summary>
        [JsonPropertyName("quote")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>lot</c>"] Lot size
        /// </summary>
        [JsonPropertyName("lot")]
        public string VolumeLotSize { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>pair_decimals</c>"] Decimals of the symbol
        /// </summary>
        [JsonPropertyName("pair_decimals")]
        public int PriceDecimals { get; set; }
        /// <summary>
        /// ["<c>lot_decimals</c>"] Lot decimals
        /// </summary>
        [JsonPropertyName("lot_decimals")]
        public int LotDecimals { get; set; }
        /// <summary>
        /// ["<c>cost_decimals</c>"] Cost decimals
        /// </summary>
        [JsonPropertyName("cost_decimals")]
        public int CostDecimals { get; set; }
        /// <summary>
        /// ["<c>lot_multiplier</c>"] Lot multiplier
        /// </summary>
        [JsonPropertyName("lot_multiplier")]
        public decimal LotMultiplier { get; set; }
        /// <summary>
        /// ["<c>leverage_buy</c>"] Buy leverage amounts
        /// </summary>
        [JsonPropertyName("leverage_buy")]
        public decimal[] LeverageBuy { get; set; } = Array.Empty<decimal>();
        /// <summary>
        /// ["<c>leverage_sell</c>"] Sell leverage amounts
        /// </summary>
        [JsonPropertyName("leverage_sell")]
        public decimal[] LeverageSell { get; set; } = Array.Empty<decimal>();
        /// <summary>
        /// ["<c>fees</c>"] Fee structure
        /// </summary>
        [JsonPropertyName("fees")]
        public KrakenFeeEntry[] Fees { get; set; } = Array.Empty<KrakenFeeEntry>();
        /// <summary>
        /// ["<c>fees_maker</c>"] Maker fee structure
        /// </summary>
        [JsonPropertyName("fees_maker")]
        public KrakenFeeEntry[] FeesMaker { get; set; } = Array.Empty<KrakenFeeEntry>();

        /// <summary>
        /// ["<c>fee_volume_currency</c>"] The asset the fee is deducted from
        /// </summary>
        [JsonPropertyName("fee_volume_currency")]
        public string FeeAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>margin_call</c>"] Margin call level
        /// </summary>
        [JsonPropertyName("margin_call")]
        public int MarginCall { get; set; }
        /// <summary>
        /// ["<c>margin_stop</c>"] Stop-out/liquidation margin level
        /// </summary>
        [JsonPropertyName("margin_stop")]
        public int MarginStop { get; set; }
        /// <summary>
        /// The minimum order volume for pair
        /// </summary>
        /// <value></value>
        [JsonPropertyName("ordermin")]
        public decimal OrderMin { get; set; }
        /// <summary>
        /// ["<c>costmin</c>"] The minimum value of an order
        /// </summary>
        [JsonPropertyName("costmin")]
        public decimal? MinValue { get; set; }
        /// <summary>
        /// ["<c>tick_size</c>"] Tick size
        /// </summary>
        [JsonPropertyName("tick_size")]
        public decimal? TickSize { get; set; }
        /// <summary>
        /// ["<c>status</c>"] Status
        /// </summary>
        [JsonPropertyName("status")]
        public SymbolStatus Status { get; set; }
        /// <summary>
        /// ["<c>long_position_limit</c>"] Long position limit
        /// </summary>
        [JsonPropertyName("long_position_limit")]
        public long LongPositionLimit { get; set; }
        /// <summary>
        /// ["<c>short_position_limit</c>"] Short position limit
        /// </summary>
        [JsonPropertyName("short_position_limit")]
        public long ShortPositionLimit { get; set; }
    }
}
