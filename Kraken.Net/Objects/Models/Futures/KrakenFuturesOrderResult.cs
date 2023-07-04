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
        public string Type { get; set; }
        /// <summary>
        /// Reduced quantity
        /// </summary>
        public decimal? ReducedQuantity { get; set; }

        /// <summary>
        /// Order info
        /// </summary>
        public KrakenFuturesOrder? Order { get; set; } = null!;
        /// <summary>
        /// New order info for edit event
        /// </summary>
        public KrakenFuturesOrder? New { get; set; } = null!;
        /// <summary>
        /// Old order info for edit event
        /// </summary>
        public KrakenFuturesOrder? Old { get; set; } = null!;
    }

}
