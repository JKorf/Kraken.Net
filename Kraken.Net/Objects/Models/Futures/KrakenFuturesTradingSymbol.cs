using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Futures
{
    /// <summary>
    /// Account-specific instrument specifications returned by the authenticated trading instruments endpoint.
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesTradingSymbol
    {
        /// <summary>
        /// ["<c>symbol</c>"] Native instrument symbol.
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>pair</c>"] Base and quote asset pair.
        /// </summary>
        [JsonPropertyName("pair")]
        public string Pair { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>base</c>"] Base asset.
        /// </summary>
        [JsonPropertyName("base")]
        public string BaseAsset { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>quote</c>"] Quote asset.
        /// </summary>
        [JsonPropertyName("quote")]
        public string QuoteAsset { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>type</c>"] Instrument type returned by the exchange, or null if not mapped by the existing enum.
        /// </summary>
        [JsonPropertyName("type")]
        public SymbolType? Type { get; set; }

        /// <summary>
        /// ["<c>tradeable</c>"] Whether the instrument is tradeable at the exchange, independently of account restrictions.
        /// </summary>
        [JsonPropertyName("tradeable")]
        public bool? Tradeable { get; set; }

        /// <summary>
        /// ["<c>contractSize</c>"] Contract size.
        /// </summary>
        [JsonPropertyName("contractSize")]
        public decimal? ContractSize { get; set; }

        /// <summary>
        /// ["<c>category</c>"] Instrument category.
        /// </summary>
        [JsonPropertyName("category")]
        public string? Category { get; set; }

        /// <summary>
        /// ["<c>tags</c>"] Instrument tags.
        /// </summary>
        [JsonPropertyName("tags")]
        public string[]? Tags { get; set; }

        /// <summary>
        /// ["<c>minimumTradeSize</c>"] Minimum trade size.
        /// </summary>
        [JsonPropertyName("minimumTradeSize")]
        public decimal? MinimumTradeSize { get; set; }

        /// <summary>
        /// ["<c>tickSize</c>"] Price tick size.
        /// </summary>
        [JsonPropertyName("tickSize")]
        public decimal? TickSize { get; set; }

        /// <summary>
        /// ["<c>contractValueTradePrecision</c>"] Decimal precision for contract quantities.
        /// </summary>
        [JsonPropertyName("contractValueTradePrecision")]
        public decimal? ContractValueTradePrecision { get; set; }

        /// <summary>
        /// ["<c>impactMidSize</c>"] Contract quantity used to calculate the impact mid price.
        /// </summary>
        [JsonPropertyName("impactMidSize")]
        public decimal? ImpactMidSize { get; set; }

        /// <summary>
        /// ["<c>maxPositionSize</c>"] Maximum position size.
        /// </summary>
        [JsonPropertyName("maxPositionSize")]
        public decimal? MaxPositionSize { get; set; }

        /// <summary>
        /// ["<c>openingDate</c>"] Time when trading opened.
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("openingDate")]
        public DateTime? OpeningDate { get; set; }

        /// <summary>
        /// ["<c>isin</c>"] Instrument ISIN.
        /// </summary>
        [JsonPropertyName("isin")]
        public string? Isin { get; set; }

        /// <summary>
        /// ["<c>postOnly</c>"] Whether the market only accepts post-only orders.
        /// </summary>
        [JsonPropertyName("postOnly")]
        public bool? PostOnly { get; set; }

        /// <summary>
        /// ["<c>feeScheduleUid</c>"] Fee schedule identifier.
        /// </summary>
        [JsonPropertyName("feeScheduleUid")]
        public string? FeeScheduleUid { get; set; }

        /// <summary>
        /// ["<c>rebateLevels</c>"] Exchange-defined rebate levels.
        /// </summary>
        [JsonPropertyName("rebateLevels")]
        public Dictionary<string, JsonElement>? RebateLevels { get; set; }

        /// <summary>
        /// ["<c>mtf</c>"] Whether the instrument is listed on the MTF.
        /// </summary>
        [JsonPropertyName("mtf")]
        public bool? Mtf { get; set; }

        /// <summary>
        /// ["<c>tradfi</c>"] Whether the instrument is a TradFi instrument.
        /// </summary>
        [JsonPropertyName("tradfi")]
        public bool? TradFi { get; set; }

        /// <summary>
        /// ["<c>restricted</c>"] Whether the authenticated account is restricted from trading this instrument.
        /// </summary>
        [JsonPropertyName("restricted")]
        public bool? Restricted { get; set; }

        /// <summary>
        /// ["<c>isExpired</c>"] Whether the instrument has expired.
        /// </summary>
        [JsonPropertyName("isExpired")]
        public bool? IsExpired { get; set; }

        /// <summary>
        /// ["<c>fundingRateCoefficient</c>"] Funding rate coefficient.
        /// </summary>
        [JsonPropertyName("fundingRateCoefficient")]
        public decimal? FundingRateCoefficient { get; set; }

        /// <summary>
        /// ["<c>lastTradingTime</c>"] Last trading time.
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("lastTradingTime")]
        public DateTime? LastTradingTime { get; set; }

        /// <summary>
        /// ["<c>maxOpenInterestUsd</c>"] Maximum open interest in USD.
        /// </summary>
        [JsonPropertyName("maxOpenInterestUsd")]
        public decimal? MaxOpenInterestUsd { get; set; }

        /// <summary>
        /// ["<c>maxOpenInterestShare</c>"] Maximum share of open interest.
        /// </summary>
        [JsonPropertyName("maxOpenInterestShare")]
        public decimal? MaxOpenInterestShare { get; set; }

        /// <summary>
        /// ["<c>marginLevels</c>"] Margin requirements by position size.
        /// </summary>
        [JsonPropertyName("marginLevels")]
        public KrakenFutureMarginLevel[]? MarginLevels { get; set; }

        /// <summary>
        /// ["<c>maxRelativeFundingRate</c>"] Maximum relative funding rate.
        /// </summary>
        [JsonPropertyName("maxRelativeFundingRate")]
        public decimal? MaxRelativeFundingRate { get; set; }

        /// <summary>
        /// ["<c>minRelativeFundingRate</c>"] Minimum relative funding rate.
        /// </summary>
        [JsonPropertyName("minRelativeFundingRate")]
        public decimal? MinRelativeFundingRate { get; set; }

        /// <summary>
        /// ["<c>underlying</c>"] Underlying instrument.
        /// </summary>
        [JsonPropertyName("underlying")]
        public string? Underlying { get; set; }

        /// <summary>
        /// ["<c>description</c>"] Instrument description.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// ["<c>makerProtectionMillis</c>"] Maker protection period in milliseconds.
        /// </summary>
        [JsonPropertyName("makerProtectionMillis")]
        public int? MakerProtectionMillis { get; set; }

        /// <summary>
        /// ["<c>optionType</c>"] Option type, call or put.
        /// </summary>
        [JsonPropertyName("optionType")]
        public string? OptionType { get; set; }

        /// <summary>
        /// ["<c>strikePrice</c>"] Option strike price.
        /// </summary>
        [JsonPropertyName("strikePrice")]
        public decimal? StrikePrice { get; set; }

        /// <summary>
        /// ["<c>underlyingFuture</c>"] Underlying futures instrument.
        /// </summary>
        [JsonPropertyName("underlyingFuture")]
        public string? UnderlyingFuture { get; set; }
    }
}
