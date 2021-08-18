using Kraken.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Socket
{
    /// <summary>
    /// System status
    /// </summary>
    public class KrakenStreamSystemStatus
    {
        /// <summary>
        /// Status
        /// </summary>
        [JsonConverter(typeof(SystemStatusConverter))]
        public SystemStatus Status { get; set; }
        /// <summary>
        /// Version
        /// </summary>
        public string Version { get; set; } = string.Empty;
    }
}
