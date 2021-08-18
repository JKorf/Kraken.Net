using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Socket
{
    /// <summary>
    /// Placed order result
    /// </summary>
    public class KrakenStreamPlacedOrder: KrakenSocketResponseBase
    {
        /// <summary>
        /// Order description
        /// </summary>
        [JsonProperty("descr")]
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Placed order id
        /// </summary>
        [JsonProperty("txId")]
        public string OrderId { get; set; } = string.Empty;
    }
}
