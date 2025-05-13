namespace Kraken.Net.Objects.Internal
{
    internal class KrakenResult
    {
        [JsonPropertyName("error")]
        public string[] Error { get; set; } = Array.Empty<string>();
    }

    internal class KrakenResult<T> : KrakenResult
    {
        [JsonPropertyName("result")]
        public T Result { get; set; } = default!;
    }
}
