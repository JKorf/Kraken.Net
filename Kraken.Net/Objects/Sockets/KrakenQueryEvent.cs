﻿namespace Kraken.Net.Objects.Sockets
{
    /// <summary>
    /// Kraken response to a query
    /// </summary>
    public record KrakenQueryEvent: KrakenEvent
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
