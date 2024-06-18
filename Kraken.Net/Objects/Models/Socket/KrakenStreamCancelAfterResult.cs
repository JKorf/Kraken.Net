using Kraken.Net.Objects.Sockets;
using System;

namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// Cancel after result
    /// </summary>
    public record KrakenStreamCancelAfterResult : KrakenQueryEvent
    {
        /// <summary>
        /// Current time
        /// </summary>
        public DateTime CurrentTime { get; set; }
        /// <summary>
        /// Trigger time
        /// </summary>
        public DateTime TriggerTime { get; set; }
    }
}
