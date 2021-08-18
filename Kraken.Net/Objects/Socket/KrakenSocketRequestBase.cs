using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Socket
{
    /// <summary>
    /// Socket request base
    /// </summary>
    public class KrakenSocketRequestBase
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
        /// <summary>
        /// Token
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; } = string.Empty;
    }
}
