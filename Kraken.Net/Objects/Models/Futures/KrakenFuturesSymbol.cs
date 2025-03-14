using CryptoExchange.Net.Converters.SystemTextJson;
using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Futures
{
    [SerializationModel]
    internal record KrakenFuturesSymbolResult : KrakenFuturesResult<KrakenFuturesSymbol[]>
    {
        [JsonPropertyName("instruments")]
        public override KrakenFuturesSymbol[] Data { get; set; } = new List<KrakenFuturesSymbol>();
    }

    /// <summary>
    /// Futures symbol info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesSymbol
    {
        /// <summary>
        /// Category
        /// </summary>
        [JsonPropertyName("category")]
        public string? Category { get; set; }
        /// <summary>
        /// The contract size of the Futures
        /// </summary>
        [JsonPropertyName("contractSize")]
        public decimal? ContractSize { get; set; }
        /// <summary>
        /// Trade precision for the contract (e.g. trade precision of 2 means trades are precise to the hundredth decimal place 0.01).
        /// </summary>
        [JsonPropertyName("contractValueTradePrecision")]
        public decimal? ContractValueTradePrecision { get; set; }
        /// <summary>
        /// Unique identifier of fee schedule associated with the symbol
        /// </summary>
        [JsonPropertyName("feeScheduleUid")]
        public string? FeeScheduleUid { get; set; }
        /// <summary>
        /// Funding rate coefficient
        /// </summary>
        [JsonPropertyName("fundingRateCoefficient")]
        public decimal? FundingRateCoefficient { get; set; }
        /// <summary>
        /// Amount of contract used to calculated the mid price for the mark price
        /// </summary>
        [JsonPropertyName("impactMidSize")]
        public decimal? ImpactMidSize { get; set; }
        /// <summary>
        /// Single-collateral contracts only: Contract's ISIN code
        /// </summary>
        [JsonPropertyName("isin")]
        public string? Isin { get; set; }
        /// <summary>
        /// Last trade time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("lastTradingTime")]
        public DateTime? LastTradingTime { get; set; }
        /// <summary>
        /// A list containing the margin schedules
        /// </summary>
        [JsonPropertyName("marginLevels")]
        public KrakenFutureMarginLevel[]? MarginLevels { get; set; }
        /// <summary>
        /// Maximum number of contracts that one can hold in a position
        /// </summary>
        [JsonPropertyName("maxPositionSize")]
        public decimal? MaxPositionSize { get; set; }
        /// <summary>
        /// Perpetuals only: the absolute value of the maximum permissible funding rate
        /// </summary>
        [JsonPropertyName("maxRelativeFundingRate")]
        public decimal? MaxRelativeFundingRate { get; set; }
        /// <summary>
        /// When the contract was first available for trading
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("openingDate")]
        public DateTime? OpeningDate { get; set; }
        /// <summary>
        /// True if the symbol is in post-only mode, false otherwise.
        /// </summary>
        [JsonPropertyName("postOnly")]
        public bool? PostOnly { get; set; }
        /// <summary>
        /// Margin levels for retail clients (investor category no longer eligible for trading).
        /// </summary>
        [JsonPropertyName("retailMarginLevels")]
        public KrakenFutureMarginLevel[]? RetailMarginLevels { get; set; }
        /// <summary>
        /// Symbols
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Tag for the contract (currently does not return a value).
        /// </summary>
        [JsonPropertyName("tags")]
        public string[] Tags { get; set; } = Array.Empty<string>();
        /// <summary>
        /// Tick size of the contract being traded.
        /// </summary>
        [JsonPropertyName("tickSize")]
        public decimal? TickSize { get; set; }
        /// <summary>
        /// True if the symbol can be traded, False otherwise.
        /// </summary>
        [JsonPropertyName("tradeable")]
        public bool Tradeable { get; set; }
        /// <summary>
        /// Type of the symbol
        /// </summary>

        [JsonPropertyName("type")]
        public SymbolType Type { get; set; }
        /// <summary>
        /// The underlying of the Futures
        /// </summary>
        [JsonPropertyName("underlying")]
        public string? Underlying { get; set; }
    }

    /// <summary>
    /// Margin level
    /// </summary>
    [SerializationModel]
    public record KrakenFutureMarginLevel
    {
        /// <summary>
        /// The lower limit of the number of contracts to which this margin level applies
        /// </summary>
        [JsonPropertyName("contracts")]
        public int? Contracts { get; set; }
        /// <summary>
        /// The initial margin requirement for this level
        /// </summary>
        [JsonPropertyName("initialMargin")]
        public decimal InitialMargin { get; set; }
        /// <summary>
        /// The maintenance margin requirement for this level
        /// </summary>
        [JsonPropertyName("maintenanceMargin")]
        public decimal MaintenanceMargin { get; set; }
        /// <summary>
        ///  The lower limit of the number of non-contract units (i.e. quote currency units for linear futures) to which this margin level applies
        /// </summary>
        [JsonPropertyName("numNonContractUnits")]
        public decimal NumNonContractUnits { get; set; }
    }
}
