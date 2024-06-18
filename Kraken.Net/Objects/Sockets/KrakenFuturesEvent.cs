﻿using Newtonsoft.Json;

namespace Kraken.Net.Objects.Sockets
{
    /// <summary>
    /// Socket event
    /// </summary>
    public record KrakenFuturesEvent
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
