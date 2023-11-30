using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kraken.Net.Objects.Internal
{
    /// <summary>
    /// Cancel orders request
    /// </summary>
    internal class KrakenSocketCancelOrdersRequest : KrakenSocketAuthRequest
    {
        [JsonProperty("txid")]
        public IEnumerable<string> OrderIds { get; set; } = Array.Empty<string>();
       
    }
}
