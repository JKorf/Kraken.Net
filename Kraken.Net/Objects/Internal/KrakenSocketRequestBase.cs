﻿using Newtonsoft.Json;

namespace Kraken.Net.Objects.Internal
{
    /// <summary>
    /// Socket request base
    /// </summary>
    internal class KrakenSocketRequest
    {
        /// <summary>
        /// Request id
        /// </summary>
        [JsonProperty("reqid")]
        public int RequestId { get; set; }
        /// <summary>
        /// Event
        /// </summary>
        [JsonProperty("event")]
        public string Event { get; set; } = string.Empty;
    }

    internal class KrakenSocketAuthRequest : KrakenSocketRequest
    {
        /// <summary>
        /// Token
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; } = string.Empty;
    }
}
