using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Sockets
{
    /// <summary>
    /// Kraken message event
    /// </summary>
    public class KrakenEvent
    {
        /// <summary>
        /// The message event
        /// </summary>
        [JsonProperty("event")]
        public string Event { get; set; } = null!;
    }
}
