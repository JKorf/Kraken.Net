namespace Kraken.Net.Objects.Models.Futures
{
    internal record KrakenFuturesResult
    {
        [JsonPropertyName("errors")]
        public IEnumerable<KrakenFuturesError>? Errors { get; set; }
        [JsonPropertyName("error")]
        public string? Error { get; set; }
        public bool Success => string.Equals(Result, "success", StringComparison.Ordinal);
        [JsonPropertyName("result")]
        public string? Result { get; set; }
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("serverTime")]
        public DateTime ServerTime { get; set; }
    }

    internal abstract record KrakenFuturesResult<T> : KrakenFuturesResult
    {
        [JsonPropertyName("data")]
        public abstract T Data { get; set; }
    }

    internal record KrakenFuturesError
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }
}
