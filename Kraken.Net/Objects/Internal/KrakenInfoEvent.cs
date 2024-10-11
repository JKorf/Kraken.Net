namespace Kraken.Net.Objects.Internal
{
    internal record KrakenInfoEvent
    {
        [JsonPropertyName("event")]
        public string Event { get; set; } = string.Empty;
        [JsonPropertyName("version")]
        public int Version { get; set; }
    }
}
