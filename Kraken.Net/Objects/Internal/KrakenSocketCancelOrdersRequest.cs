namespace Kraken.Net.Objects.Internal
{
    /// <summary>
    /// Cancel orders request
    /// </summary>
    internal class KrakenSocketCancelOrdersRequest : KrakenSocketAuthRequest
    {
        [JsonPropertyName("txid")]
        public IEnumerable<string> OrderIds { get; set; } = Array.Empty<string>();
       
    }
}
