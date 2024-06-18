using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Futures
{
    /// <summary>
    /// User execution events
    /// </summary>
    public record KrakenFuturesUserExecutionEvents
    {
        /// <summary>
        /// Account id
        /// </summary>
        public string AccountUid { get; set; } = string.Empty;
        /// <summary>
        /// Continuation token for pagination
        /// </summary>
        public string ContinuationToken { get; set; } = string.Empty;
        /// <summary>
        /// Total number of results
        /// </summary>
        [JsonProperty("len")]
        public long Total { get; set; }
        /// <summary>
        /// Server time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime ServerTime { get; set; }
        /// <summary>
        /// Elements
        /// </summary>
        public IEnumerable<KrakenFuturesExecutionElement> Elements { get; set; } = Array.Empty<KrakenFuturesExecutionElement>();
    }

    /// <summary>
    /// Execution event info
    /// </summary>
    public record KrakenFuturesExecutionElement
    {
        /// <summary>
        /// Uid
        /// </summary>
        public string Uid { get; set; } = string.Empty;
        /// <summary>
        /// Event timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Event info
        /// </summary>
        public KrakenFuturesExecutionEventWrapper Event { get; set; } = null!;
    }

    /// <summary>
    /// Event info
    /// </summary>
    public record KrakenFuturesExecutionEventWrapper
    {
        /// <summary>
        /// Execution info
        /// </summary>
        public KrakenFuturesExecutionEvent Execution { get; set; } = null!;
    }

    /// <summary>
    /// Execution info
    /// </summary>
    public record KrakenFuturesExecutionEvent
    {
        /// <summary>
        /// Execution info
        /// </summary>
        public KrakenFuturesExecution Execution { get; set; } = null!;

        /// <summary>
        /// Taker reduced quantity
        /// </summary>
        public decimal? TakerReducedQuantity { get; set; }
    }

    /// <summary>
    /// Execution info
    /// </summary>
    public record KrakenFuturesExecution
    {
        /// <summary>
        /// Limit filled
        /// </summary>
        public bool? LimitFilled { get; set; }
        /// <summary>
        /// Maker order info
        /// </summary>
        public KrakenFuturesExecutionOrder? MakerOrder { get; set; }
        /// <summary>
        /// Maker order data
        /// </summary>
        public KrakenFuturesOrderData? MakerOrderData { get; set; }
        /// <summary>
        /// Mark price
        /// </summary>
        public decimal? MarkPrice { get; set; }
        /// <summary>
        /// Old taker order
        /// </summary>
        public KrakenFuturesExecutionOrder? OldTakerOrder { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        public decimal? Price { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        public decimal? Quantity { get; set; }
        /// <summary>
        /// Taker order
        /// </summary>
        public KrakenFuturesExecutionOrder? TakerOrder { get; set; }
        /// <summary>
        /// Taker order data
        /// </summary>
        public KrakenFuturesOrderData? TakerOrderData { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Timestamp { get; set; }
        /// <summary>
        /// Uid
        /// </summary>
        public string Uid { get; set; } = string.Empty;
        /// <summary>
        /// Usd value
        /// </summary>
        public decimal? UsdValue { get; set; }
    }

    /// <summary>
    /// Additional order data
    /// </summary>
    public record KrakenFuturesOrderData
    {
        /// <summary>
        /// Fee
        /// </summary>
        public decimal Fee { get; set; }

        /// <summary>
        /// Position size
        /// </summary>
        public decimal PositionSize { get; set; }
    }

    /// <summary>
    /// Excecution order info
    /// </summary>
    public record KrakenFuturesExecutionOrder
    {
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonProperty("clientId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Account id
        /// </summary>
        [JsonProperty("accountUid")]
        public string AccountUid { get; set; } = string.Empty;
        /// <summary>
        /// Tradeable
        /// </summary>
        [JsonProperty("tradeable")]
        public string Tradeable { get; set; } = string.Empty;
        /// <summary>
        /// Quantity filled
        /// </summary>
        [JsonProperty("filled")]
        public decimal QuantityFilled { get; set; }
        /// <summary>
        /// Last update time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("lastUpdateTimestamp")]
        public DateTime? LastUpdateTime { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonProperty("limitPrice")]
        public decimal? Price { get; set; }
        /// <summary>
        /// Order id
        /// </summary>
        [JsonProperty("uid")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Quantity
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Reduce only
        /// </summary>
        public bool ReduceOnly { get; set; }
        /// <summary>
        /// Order side
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        [JsonProperty("direction")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Order type
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        [JsonProperty("orderType")]
        public FuturesOrderType Type { get; set; }
    }
}
