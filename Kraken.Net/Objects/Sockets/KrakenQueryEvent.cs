using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Sockets
{
    /// <summary>
    /// Kraken response to a query
    /// </summary>
    public class KrakenQueryEvent: KrakenEvent
    {
        /// <summary>
        /// Response status
        /// </summary>
        public string Status { get; set; } = string.Empty;
        /// <summary>
        /// Optional error message 
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
}
