using Kraken.Net.Objects.Models.Socket;

namespace Kraken.Net.Objects.Internal
{
    internal class KrakenSocketPlaceMultipleOrderRequestV2 : KrakenSocketAuthRequestV2
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        [JsonPropertyName("validate"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? ValidateOnly { get; set; }
        [JsonPropertyName("deadline"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Deadline { get; set; }
        [JsonPropertyName("orders")]
        public KrakenSocketOrderRequest[] Orders { get; set; } = Array.Empty<KrakenSocketOrderRequest>();
    }

}
