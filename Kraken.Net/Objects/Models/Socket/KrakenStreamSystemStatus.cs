using Kraken.Net.Converters;
using Kraken.Net.Enums;
using Newtonsoft.Json;

namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// System status
    /// </summary>
    public class KrakenStreamSystemStatus
    {
        /// <summary>
        /// Connection id
        /// </summary>
        public string ConnectionId { get; set; } = string.Empty;
        /// <summary>
        /// Name of the event
        /// </summary>
        public string Event { get; set; } = string.Empty;
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
