using CryptoExchange.Net.Converters;
using Kraken.Net.Enums;
using Kraken.Net.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Futures
{
    internal record KrakenFuturesOpenOrderResult : KrakenFuturesResult<IEnumerable<KrakenFuturesOpenOrder>>
    {
        [JsonProperty("openOrders")]
        public override IEnumerable<KrakenFuturesOpenOrder> Data { get; set; } = Array.Empty<KrakenFuturesOpenOrder>();
    }

    /// <summary>
    /// Order info
    /// </summary>
    public record KrakenFuturesOpenOrder : IKrakenFuturesOrder
    {
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonProperty("cliOrdId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Quantity filled
        /// </summary>
        [JsonProperty("filledSize")]
        public decimal QuantityFilled { get; set; }
        /// <summary>
        /// Last update time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("lastUpdateTime")]
        public DateTime? LastUpdateTime { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonProperty("limitPrice")]
        public decimal? Price { get; set; }
        /// <summary>
        /// Order id
        /// </summary>
        [JsonProperty("order_id")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Quantity
        /// </summary>
        public decimal Quantity
        { 
            get { return QuantityRemaining + QuantityFilled; }
            set { }
        }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty("unfilledSize")]
        public decimal QuantityRemaining { get; set; }
        /// <summary>
        /// Reduce only
        /// </summary>
        public bool ReduceOnly { get; set; }
        /// <summary>
        /// Order side
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Symbol
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("receivedTime")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Order type
        /// </summary>
        [JsonProperty("orderType")]
        [JsonConverter(typeof(EnumConverter))]
        public FuturesOrderType Type { get; set; }
        /// <summary>
        /// Order status
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        public OpenOrderStatus Status { get; set; }
        /// <summary>
        /// Trigger signal
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        public TriggerSignal? TriggerSignal { get; set; }
        /// <summary>
        /// Stop price
        /// </summary>
        public decimal? StopPrice { get; set; }
    }
}
