using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Sockets
{
    internal class KrakenFuturesResponse
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
        /// Message
        /// </summary>
        [JsonProperty("message")]
        public string? Message { get; set; }

        /// <summary>
        /// The symbols
        /// </summary>
        [JsonProperty("product_ids")]
        public List<string>? Symbols { get; set; }
    }
}
