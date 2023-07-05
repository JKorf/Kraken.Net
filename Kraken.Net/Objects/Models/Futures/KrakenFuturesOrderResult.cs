using CryptoExchange.Net.Converters;
using Kraken.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Futures
{
    internal class KrakenFuturesOrderCancelResult : KrakenFuturesResult<KrakenFuturesOrderResult>
    {
        [JsonProperty("cancelStatus")]
        public override KrakenFuturesOrderResult Data { get; set; } = null!;
    }

    internal class KrakenFuturesOrderPlaceResult : KrakenFuturesResult<KrakenFuturesOrderResult>
    {
        [JsonProperty("sendStatus")]
        public override KrakenFuturesOrderResult Data { get; set; } = null!;
    }

    internal class KrakenFuturesOrderEditResult : KrakenFuturesResult<KrakenFuturesOrderResult>
    {
        [JsonProperty("editStatus")]
        public override KrakenFuturesOrderResult Data { get; set; } = null!;
    }

    /// <summary>
    /// Order status info
    /// </summary>
    public class KrakenFuturesOrderResult
    {
        /// <summary>
        /// Order id
        /// </summary>
        [JsonProperty("order_id")]
        public string OrderId { get; set; } = string.Empty;

        [JsonProperty("orderId")]
        internal string OrderIdInternal
        {
            get => OrderId;
            set => OrderId = value;
        }

        /// <summary>
        /// Receive timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime ReceivedTime { get; set; }
        /// <summary>
        /// Order status
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        public KrakenFuturesOrderActionStatus Status { get; set; }
        /// <summary>
        /// Order events
        /// </summary>
        public IEnumerable<KrakenFuturesOrderEvent> OrderEvents { get; set; } = Array.Empty<KrakenFuturesOrderEvent>();
    }

    /// <summary>
    /// Order event
    /// </summary>
    public class KrakenFuturesOrderEvent
    {
        /// <summary>
        /// Event type
        /// </summary>
        public string Type { get; set; } = string.Empty;
        /// <summary>
        /// Reduced quantity
        /// </summary>
        public decimal? ReducedQuantity { get; set; }
        /// <summary>
        /// Uid
        /// </summary>
        public string? Uid { get; set; }
        /// <summary>
        /// Execution id
        /// </summary>
        public string? ExecutionId { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty("amount")]
        public decimal? Quantity { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty("price")]
        public decimal? Price { get; set; }

        /// <summary>
        /// Order info
        /// </summary>
        public KrakenFuturesOrder? Order { get; set; }
        /// <summary>
        /// New order info for edit event
        /// </summary>
        public KrakenFuturesOrder? New { get; set; }
        /// <summary>
        /// Old order info for edit event
        /// </summary>
        public KrakenFuturesOrder? Old { get; set; }
        /// <summary>
        /// Order before execution
        /// </summary>
        [JsonProperty("orderPriorExecution")]
        public KrakenFuturesOrder? OrderBeforeExecution { get; set; }
        /// <summary>
        /// Order before edit
        /// </summary>
        [JsonProperty("orderPriorEdit")]
        public KrakenFuturesOrder? OrderBeforeEdit { get; set; }
    }

}
