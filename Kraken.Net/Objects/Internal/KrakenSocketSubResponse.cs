namespace Kraken.Net.Objects.Internal
{
    internal class KrakenSocketSubResponse
    {
        [JsonPropertyName("channel")]
        public string Channel { get; set; } = string.Empty;
        [JsonPropertyName("snapshot")]
        public bool Snapshot { get; set; }
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
    }
}
