using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Futures
{
    internal class KrakenFuturesCancelledOrdersResult : KrakenFuturesResult<KrakenFuturesCancelledOrders>
    {
        [JsonProperty("cancelStatus")]
        public override KrakenFuturesCancelledOrders Data { get; set; } = null!;
    }

    /// <summary>
    /// Cancelled order info
    /// </summary>
    public class KrakenFuturesCancelledOrders
    {
        /// <summary>
        /// Cancelled all or a specific symbol
        /// </summary>
        public string CancelOnly { get; set; } = string.Empty;
        /// <summary>
        /// Cancelled order ids
        /// </summary>
        public IEnumerable<KrakenFuturesOrderId> CancelledOrders { get; set; } = Array.Empty<KrakenFuturesOrderId>();
        /// <summary>
        /// Order events
        /// </summary>
        public IEnumerable<KrakenFuturesOrderEvent> OrderEvents { get; set; } = Array.Empty<KrakenFuturesOrderEvent>();
        /// <summary>
        /// Received timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime ReceivedTime { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; } = string.Empty;
    }
}
