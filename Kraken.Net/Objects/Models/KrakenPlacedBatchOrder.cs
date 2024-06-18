using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Batch order result
    /// </summary>
    public record KrakenBatchOrderResult
    {
        /// <summary>
        /// Orders
        /// </summary>
        [JsonProperty("orders")]
        public IEnumerable<KrakenPlacedBatchOrder> Orders { get; set; } = Array.Empty<KrakenPlacedBatchOrder>();
    }

    /// <summary>
    /// Placed batch order
    /// </summary>
    public record KrakenPlacedBatchOrder
    {
        /// <summary>
        /// Order id
        /// </summary>
        [JsonProperty("txid")]
        public string OrderId { get; set; } = null!;
        /// <summary>
        /// Description
        /// </summary>
        [JsonProperty("descr")]
        public KrakenPlacedOrderDescription Description { get; set; } = null!;
        /// <summary>
        /// Close order description
        /// </summary>
        [JsonProperty("close")]
        public string? CloseOrderInfo { get; set; }
    }
}
