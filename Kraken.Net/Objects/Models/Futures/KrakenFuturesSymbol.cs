using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Futures
{
    [SerializationModel]
    internal record KrakenFuturesSymbolResult : KrakenFuturesResult<KrakenFuturesSymbol[]>
    {
        [JsonPropertyName("instruments")]
        public override KrakenFuturesSymbol[] Data { get; set; } = [];
    }

    /// <summary>
    /// Futures symbol info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesSymbol
    {
        /// <summary>
        /// ["<c>category</c>"] Category
        /// </summary>
        [JsonPropertyName("category")]
        public string? Category { get; set; }
        /// <summary>
        /// ["<c>contractSize</c>"] The contract size of the Futures
        /// </summary>
        [JsonPropertyName("contractSize")]
        public decimal? ContractSize { get; set; }
        /// <summary>
        /// ["<c>contractValueTradePrecision</c>"] Trade precision for the contract (e.g. trade precision of 2 means trades are precise to the hundredth decimal place 0.01).
        /// </summary>
        [JsonPropertyName("contractValueTradePrecision")]
        public decimal? ContractValueTradePrecision { get; set; }
        /// <summary>
        /// ["<c>feeScheduleUid</c>"] Unique identifier of fee schedule associated with the symbol
        /// </summary>
        [JsonPropertyName("feeScheduleUid")]
        public string? FeeScheduleUid { get; set; }
        /// <summary>
        /// ["<c>fundingRateCoefficient</c>"] Funding rate coefficient
        /// </summary>
        [JsonPropertyName("fundingRateCoefficient")]
        public decimal? FundingRateCoefficient { get; set; }
        /// <summary>
        /// ["<c>impactMidSize</c>"] Amount of contract used to calculated the mid price for the mark price
        /// </summary>
        [JsonPropertyName("impactMidSize")]
        public decimal? ImpactMidSize { get; set; }
        /// <summary>
        /// ["<c>isin</c>"] Single-collateral contracts only: Contract's ISIN code
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
        /// ["<c>marginLevels</c>"] A list containing the margin schedules
        /// </summary>
        [JsonPropertyName("marginLevels")]
        public KrakenFutureMarginLevel[]? MarginLevels { get; set; }
        /// <summary>
        /// ["<c>maxPositionSize</c>"] Maximum number of contracts that one can hold in a position
        /// </summary>
        [JsonPropertyName("maxPositionSize")]
        public decimal? MaxPositionSize { get; set; }
        /// <summary>
        /// ["<c>maxRelativeFundingRate</c>"] Perpetuals only: the absolute value of the maximum permissible funding rate
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
        /// ["<c>postOnly</c>"] True if the symbol is in post-only mode, false otherwise.
        /// </summary>
        [JsonPropertyName("postOnly")]
        public bool? PostOnly { get; set; }
        /// <summary>
        /// ["<c>retailMarginLevels</c>"] Margin levels for retail clients (investor category no longer eligible for trading).
        /// </summary>
        [JsonPropertyName("retailMarginLevels")]
        public KrakenFutureMarginLevel[]? RetailMarginLevels { get; set; }
        /// <summary>
        /// ["<c>symbol</c>"] Symbols
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>pair</c>"] Trading pair
        /// </summary>
        [JsonPropertyName("pair")]
        public string Pair { get; set; } = string.Empty;
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
        /// ["<c>tags</c>"] Tag for the contract (currently does not return a value).
        /// </summary>
        [JsonPropertyName("tags")]
        public string[] Tags { get; set; } = Array.Empty<string>();
        /// <summary>
        /// ["<c>tickSize</c>"] Tick size of the contract being traded.
        /// </summary>
        [JsonPropertyName("tickSize")]
        public decimal? TickSize { get; set; }
        /// <summary>
        /// ["<c>tradeable</c>"] True if the symbol can be traded, False otherwise.
        /// </summary>
        [JsonPropertyName("tradeable")]
        public bool Tradeable { get; set; }
        /// <summary>
        /// ["<c>type</c>"] Type of the symbol
        /// </summary>

        [JsonPropertyName("type")]
        public SymbolType Type { get; set; }
        /// <summary>
        /// ["<c>underlying</c>"] The underlying of the Futures
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
        /// ["<c>contracts</c>"] The lower limit of the number of contracts to which this margin level applies
        /// </summary>
        [JsonPropertyName("contracts")]
        public int? Contracts { get; set; }
        /// <summary>
        /// ["<c>initialMargin</c>"] The initial margin requirement for this level
        /// </summary>
        [JsonPropertyName("initialMargin")]
        public decimal InitialMargin { get; set; }
        /// <summary>
        /// ["<c>maintenanceMargin</c>"] The maintenance margin requirement for this level
        /// </summary>
        [JsonPropertyName("maintenanceMargin")]
        public decimal MaintenanceMargin { get; set; }
        /// <summary>
        /// ["<c>numNonContractUnits</c>"] The lower limit of the number of non-contract units (i.e. quote currency units for linear futures) to which this margin level applies
        /// </summary>
        [JsonPropertyName("numNonContractUnits")]
        public decimal NumNonContractUnits { get; set; }
    }
}
