using Kraken.Net.Converters;
using Newtonsoft.Json;
using System;

namespace Kraken.Net.Objects
{
    /// <summary>
    /// System status
    /// </summary>
    public class KrakenSystemStatus
    {
        /// <summary>
        /// Platform status
        /// </summary>
        [JsonConverter(typeof(SystemStatusConverter))]
        public SystemStatus Status { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
