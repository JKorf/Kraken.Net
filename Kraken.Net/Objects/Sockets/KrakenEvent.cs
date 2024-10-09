namespace Kraken.Net.Objects.Sockets
{
    /// <summary>
    /// Kraken message event
    /// </summary>
    public record KrakenEvent
    {
        /// <summary>
        /// The message event
        /// </summary>
        [JsonPropertyName("event")]
        public string Event { get; set; } = null!;
    }
}
