using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Interfaces;
using Kraken.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Snapshot book update
    /// </summary>
    public class KrakenFuturesOpenOrdersSnapshotUpdate : KrakenFuturesUpdateMessage
    {
        /// <summary>
        /// Account id
        /// </summary>
        public string Account { get; set; } = string.Empty;
        /// <summary>
        /// Current open orders
        /// </summary>
        public IEnumerable<KrakenFuturesSocketOpenOrder> Orders { get; set; } = Array.Empty<KrakenFuturesSocketOpenOrder>();
    }

    /// <summary>
    /// Open order update
    /// </summary>
    public class KrakenFuturesOpenOrdersUpdate : KrakenFuturesUpdateMessage
    {
        /// <summary>
        /// Is cancel
        /// </summary>
        [JsonProperty("is_cancel")]
        public bool IsCancel { get; set; }
        /// <summary>
        /// Reason
        /// </summary>
        public string Reason { get; set; } = string.Empty;
        /// <summary>
        /// Reason
        /// </summary>
        [JsonProperty("order_id")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Order info
        /// </summary>
        public KrakenFuturesSocketOpenOrder? Order { get; set; } = null!;
    }

    /// <summary>
    /// Open order
    /// </summary>
    public class KrakenFuturesSocketOpenOrder
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonProperty("instrument")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonProperty("time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("last_update_time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime LastUpdateTime { get; set; }
        /// <summary>
        /// Quantitiy
        /// </summary>
        [JsonProperty("qty")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Filled quantity
        /// </summary>
        [JsonProperty("filled")]
        public decimal QuantityFilled { get; set; }
        /// <summary>
        /// Limit price
        /// </summary>
        [JsonProperty("limit_price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Stop price
        /// </summary>
        [JsonProperty("stop_price")]
        public decimal? StopPrice { get; set; }
        /// <summary>
        /// Order type
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
        /// <summary>
        /// Order id
        /// </summary>
        [JsonProperty("order_id")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonProperty("cli_ord_id")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Side
        /// </summary>
        [JsonProperty("direction")]
        [JsonConverter(typeof(EnumConverter))]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Reduce only
        /// </summary>
        [JsonProperty("reduce_only")]
        public bool ReduceOnly { get; set; }
        /// <summary>
        /// Trigger signal
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        public TriggerSignal TriggerSignal { get; set; }
        /// <summary>
        /// Trailing stop options
        /// </summary>
        [JsonProperty("trailing_stop_options")]
        public KrakenFuturesTrailingStopOptions? TrailingStopOptions { get; set; }
    }

    /// <summary>
    /// Trailing stop options
    /// </summary>
    public class KrakenFuturesTrailingStopOptions
    {
        /// <summary>
        /// Max deviation
        /// </summary>
        [JsonProperty("max_deviation")]
        public decimal? MaxDeviation { get; set; }
        /// <summary>
        /// Unit
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        public TrailingStopDeviationUnit Unit { get; set; }
    }
}
