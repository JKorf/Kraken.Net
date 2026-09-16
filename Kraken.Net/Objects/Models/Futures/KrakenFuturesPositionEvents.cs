using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Futures
{
    /// <summary>
    /// Futures position events
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesPositionEvents
    {
        /// <summary>Account id</summary>
        [JsonPropertyName("accountUid")]
        public string AccountUid { get; set; } = string.Empty;
        /// <summary>Continuation token for pagination</summary>
        [JsonPropertyName("continuationToken")]
        public string? ContinuationToken { get; set; }
        /// <summary>Number of returned events</summary>
        [JsonPropertyName("len")]
        public long Total { get; set; }
        /// <summary>Server time</summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("serverTime")]
        public DateTime ServerTime { get; set; }
        /// <summary>Position events</summary>
        [JsonPropertyName("elements")]
        public KrakenFuturesPositionEventElement[] Elements { get; set; } = Array.Empty<KrakenFuturesPositionEventElement>();
    }

    /// <summary>
    /// Futures position event element
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesPositionEventElement
    {
        /// <summary>Event id</summary>
        [JsonPropertyName("uid")]
        public string Uid { get; set; } = string.Empty;
        /// <summary>Event time</summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>Event data</summary>
        [JsonPropertyName("event")]
        public KrakenFuturesPositionEventWrapper Event { get; set; } = null!;
    }

    /// <summary>
    /// Futures position event wrapper
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesPositionEventWrapper
    {
        /// <summary>Position update</summary>
        [JsonPropertyName("PositionUpdate")]
        public KrakenFuturesPositionEvent PositionUpdate { get; set; } = null!;
    }

    /// <summary>
    /// Futures position event
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesPositionEvent
    {
        /// <summary>Account id</summary>
        [JsonPropertyName("accountUid")]
        public string AccountUid { get; set; } = string.Empty;
        /// <summary>Symbol</summary>
        [JsonPropertyName("tradeable")]
        public string Tradeable { get; set; } = string.Empty;
        /// <summary>Previous position size</summary>
        [JsonPropertyName("oldPosition")]
        public decimal OldPosition { get; set; }
        /// <summary>Previous average entry price</summary>
        [JsonPropertyName("oldAverageEntryPrice")]
        public decimal? OldAverageEntryPrice { get; set; }
        /// <summary>New position size</summary>
        [JsonPropertyName("newPosition")]
        public decimal NewPosition { get; set; }
        /// <summary>New average entry price</summary>
        [JsonPropertyName("newAverageEntryPrice")]
        public decimal? NewAverageEntryPrice { get; set; }
        /// <summary>Fill time</summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("fillTime")]
        public DateTime? FillTime { get; set; }
        /// <summary>Fee</summary>
        [JsonPropertyName("fee")]
        public decimal? Fee { get; set; }
        /// <summary>Fee asset</summary>
        [JsonPropertyName("feeCurrency")]
        public string? FeeAsset { get; set; }
        /// <summary>Realized profit and loss</summary>
        [JsonPropertyName("realizedPnL")]
        public decimal? RealizedProfitAndLoss { get; set; }
        /// <summary>Position change</summary>
        [JsonPropertyName("positionChange")]
        public KrakenFuturesPositionChange PositionChange { get; set; }
        /// <summary>Execution id</summary>
        [JsonPropertyName("executionUid")]
        public string? ExecutionId { get; set; }
        /// <summary>Execution price</summary>
        [JsonPropertyName("executionPrice")]
        public decimal? ExecutionPrice { get; set; }
        /// <summary>Execution size</summary>
        [JsonPropertyName("executionSize")]
        public decimal? ExecutionSize { get; set; }
        /// <summary>Trade type</summary>
        [JsonPropertyName("tradeType")]
        public KrakenFuturesPositionTradeType? TradeType { get; set; }
        /// <summary>Funding realization time</summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("fundingRealizationTime")]
        public DateTime? FundingRealizationTime { get; set; }
        /// <summary>Realized funding</summary>
        [JsonPropertyName("realizedFunding")]
        public decimal? RealizedFunding { get; set; }
        /// <summary>Settlement price</summary>
        [JsonPropertyName("settlementPrice")]
        public decimal? SettlementPrice { get; set; }
        /// <summary>Event time</summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>Update reason</summary>
        [JsonPropertyName("updateReason")]
        public KrakenFuturesPositionUpdateReason UpdateReason { get; set; }
    }
}
