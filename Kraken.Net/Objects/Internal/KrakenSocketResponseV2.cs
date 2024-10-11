namespace Kraken.Net.Objects.Internal
{
    internal class KrakenSocketResponseV2<T>
    {
        [JsonPropertyName("method")]
        public string Method { get; set; } = string.Empty;
        [JsonPropertyName("result")]
        public T Result { get; set; } = default!;
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("time_in")]
        public DateTime TimestampIn { get; set; }
        [JsonPropertyName("time_out")]
        public DateTime TimestampOut { get; set; }
        [JsonPropertyName("req_id")]
        public long RequestId { get; set; }
        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }
}
