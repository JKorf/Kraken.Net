using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Futures
{
    /// <summary>
    /// User execution events
    /// </summary>
    public class KrakenFuturesUserExecutionEvents
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
    public class KrakenFuturesExecutionElement
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
    public class KrakenFuturesExecutionEventWrapper
    {
        /// <summary>
        /// Execution info
        /// </summary>
        public KrakenFuturesExecutionEvent Execution { get; set; } = null!;
    }

    /// <summary>
    /// Execution info
    /// </summary>
    public class KrakenFuturesExecutionEvent
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
    public class KrakenFuturesExecution
    {
        /// <summary>
        /// Limit filled
        /// </summary>
        public decimal? LimitFilled { get; set; }
        /// <summary>
        /// Maker order info
        /// </summary>
        public KrakenFuturesOrder? MakerOrder { get; set; }
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
        public KrakenFuturesOrder? OldTakerOrder { get; set; }
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
        public KrakenFuturesOrder? TakerOrder { get; set; }
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
    public class KrakenFuturesOrderData
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
}
