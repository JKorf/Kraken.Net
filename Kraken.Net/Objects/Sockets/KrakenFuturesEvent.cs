using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Sockets
{
    public class KrakenFuturesEvent
    {
        /// <summary>
        /// The event type
        /// </summary>
        [JsonProperty("event")]
        public string Event { get; set; } = string.Empty;
        /// <summary>
        /// The feed
        /// </summary>
        [JsonProperty("feed")]
        public string Feed { get; set; } = string.Empty;

        /// <summary>
        /// The symbols
        /// </summary>
        [JsonProperty("product_id")]
        public string? Symbol { get; set; }
    }
}
