﻿namespace Kraken.Net.Objects.Sockets
{
    /// <summary>
    /// Kraken message event
    /// </summary>
    public record KrakenEvent
    {
        /// <summary>
        /// The channel
        /// </summary>
        [JsonPropertyName("channel")]
        public string Channel { get; set; } = null!;
    }
}
