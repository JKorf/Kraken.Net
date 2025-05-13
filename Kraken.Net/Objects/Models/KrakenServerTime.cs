using CryptoExchange.Net.Converters.SystemTextJson;
namespace Kraken.Net.Objects.Models
{
    [SerializationModel]
    internal record KrakenServerTime
    {
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("unixTime")]
        public DateTime UnixTime { get; set; }
        [JsonPropertyName("rfc1123")]
        public string RfcTime { get; set; } = string.Empty;
    }
}
