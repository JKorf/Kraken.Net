using CryptoExchange.Net.Converters;
using Kraken.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Socket
{
    /// <summary>
    /// Cancel orders request
    /// </summary>
    internal class KrakenSocketCancelOrdersRequest : KrakenSocketRequestBase
    {
        [JsonProperty("txid")]
        public IEnumerable<string> OrderIds { get; set; } = Array.Empty<string>();
       
    }
}
