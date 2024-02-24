using CryptoExchange.Net.Converters;
using Kraken.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Futures
{
    internal class KrakenFuturesOrderStatusResult : KrakenFuturesResult<IEnumerable<KrakenFuturesOrderStatus>>
    {
        [JsonProperty("orders")]
        public override IEnumerable<KrakenFuturesOrderStatus> Data { get; set; } = Array.Empty<KrakenFuturesOrderStatus>();
    }

    /// <summary>
    /// Order status info
    /// </summary>
    public class KrakenFuturesOrderStatus
    {
        /// <summary>
        /// Order error
        /// </summary>
        public string? Error { get; set; }
        /// <summary>
        /// Order details
        /// </summary>
        public KrakenFuturesCachedOrder Order { get; set; } = null!;
        /// <summary>
        /// Order status
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        public KrakenFuturesOrderActiveStatus Status { get; set; }
        /// <summary>
        /// Update reason
        /// </summary>
        public string? UpdateReason { get; set; }
    }
}
