namespace Kraken.Net.Objects.Internal
{
    internal class KrakenSocketUpdateV2<T>
    {
        [JsonPropertyName("channel")]
        public string Channel { get; set; } = string.Empty;
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        [JsonPropertyName("timestamp")]
        public DateTime? Timestamp { get; set; }
        [JsonPropertyName("data")]
        public T Data { get; set; } = default!;
    }
}
