using CryptoExchange.Net.Converters;
using Kraken.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Socket
{
    /// <summary>
    /// Place order request
    /// </summary>
    internal class KrakenSocketCancelAfterRequest : KrakenSocketRequestBase
    {
        /// <summary>
        /// Timeout
        /// </summary>
        [JsonProperty("timeout")]
        public int Timeout { get; set; }
    }
}
