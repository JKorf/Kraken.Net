using CryptoExchange.Net.Converters.SystemTextJson;
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
        /// Alternative name
        /// </summary>
        [JsonPropertyName("altname")]
        public string AlternateName { get; set; } = string.Empty;
        /// <summary>
        /// Name to use for the socket client subscriptions
        /// </summary>
        [JsonPropertyName("wsname")]
        public string WebsocketName { get; set; } = string.Empty;
        /// <summary>
        /// Class of the base asset
        /// </summary>
        [JsonPropertyName("aclass_base")]
        public string BaseAssetClass { get; set; } = string.Empty;
        /// <summary>
        /// Name of the base asset
        /// </summary>
        [JsonPropertyName("base")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// Class of the quote asset
        /// </summary>
        [JsonPropertyName("aclass_quote")]
        public string QuoteAssetClass { get; set; } = string.Empty;
        /// <summary>
        /// Name of the quote asset
        /// </summary>
        [JsonPropertyName("quote")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// Lot size
        /// </summary>
        [JsonPropertyName("lot")]
        public string VolumeLotSize { get; set; } = string.Empty;
        /// <summary>
        /// Decimals of the symbol
        /// </summary>
        [JsonPropertyName("pair_decimals")]
        public int PriceDecimals { get; set; }
        /// <summary>
        /// Lot decimals
        /// </summary>
        [JsonPropertyName("lot_decimals")]
        public int LotDecimals { get; set; }
        /// <summary>
        /// Cost decimals
        /// </summary>
        [JsonPropertyName("cost_decimals")]
        public int CostDecimals { get; set; }
        /// <summary>
        /// Lot multiplier
        /// </summary>
        [JsonPropertyName("lot_multiplier")]
        public decimal LotMultiplier { get; set; }
        /// <summary>
        /// Buy leverage amounts
        /// </summary>
        [JsonPropertyName("leverage_buy")]
        public decimal[] LeverageBuy { get; set; } = Array.Empty<decimal>();
        /// <summary>
        /// Sell leverage amounts
        /// </summary>
        [JsonPropertyName("leverage_sell")]
        public decimal[] LeverageSell { get; set; } = Array.Empty<decimal>();
        /// <summary>
        /// Fee structure
        /// </summary>
        [JsonPropertyName("fees")]
        public KrakenFeeEntry[] Fees { get; set; } = Array.Empty<KrakenFeeEntry>();
        /// <summary>
        /// Maker fee structure
        /// </summary>
        [JsonPropertyName("fees_maker")]
        public KrakenFeeEntry[] FeesMaker { get; set; } = Array.Empty<KrakenFeeEntry>();

        /// <summary>
        /// The asset the fee is deducted from
        /// </summary>
        [JsonPropertyName("fee_volume_currency")]
        public string FeeAsset { get; set; } = string.Empty;
        /// <summary>
        /// Margin call level
        /// </summary>
        [JsonPropertyName("margin_call")]
        public int MarginCall { get; set; }
        /// <summary>
        /// Stop-out/liquidation margin level
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
        /// The minimum value of an order
        /// </summary>
        [JsonPropertyName("costmin")]
        public decimal? MinValue { get; set; }
        /// <summary>
        /// Tick size
        /// </summary>
        [JsonPropertyName("tick_size")]
        public decimal? TickSize { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("status")]
        public SymbolStatus Status { get; set; }
        /// <summary>
        /// Long position limit
        /// </summary>
        [JsonPropertyName("long_position_limit")]
        public long LongPositionLimit { get; set; }
        /// <summary>
        /// Short position limit
        /// </summary>
        [JsonPropertyName("short_position_limit")]
        public long ShortPositionLimit { get; set; }
    }
}
