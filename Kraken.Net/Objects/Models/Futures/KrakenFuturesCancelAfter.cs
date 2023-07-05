using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Models.Futures
{
    internal class KrakenFuturesCancelAfterResult : KrakenFuturesResult<KrakenFuturesCancelAfter>
    {
        [JsonProperty("status")]
        public override KrakenFuturesCancelAfter Data { get; set; } = null!;
    }

    /// <summary>
    /// Cancel after info
    /// </summary>
    public class KrakenFuturesCancelAfter
    {
        /// <summary>
        /// Current timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime CurrentTime { get; set; }
        /// <summary>
        /// Trigger time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? TriggerTime { get; set; }
    }
}
