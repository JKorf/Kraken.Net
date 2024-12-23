namespace Kraken.Net.Objects.Internal
{
    internal class KrakenSocketRequestV2
    {
        [JsonPropertyName("method")]
        public string Method { get; set; } = string.Empty;
        [JsonPropertyName("req_id")]
        public long RequestId { get; set; }
    }

    internal class KrakenSocketRequestV2<T> : KrakenSocketRequestV2
    {
        [JsonPropertyName("params")]
        public T Parameters { get; set; } = default!;
    }
}
