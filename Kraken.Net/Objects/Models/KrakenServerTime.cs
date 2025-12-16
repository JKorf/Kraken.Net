namespace Kraken.Net.Objects.Models
{
    [SerializationModel]
    internal record KrakenServerTime
    {
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("unixtime")]
        public DateTime UnixTime { get; set; }
        [JsonPropertyName("rfc1123")]
        public string RfcTime { get; set; } = string.Empty;
    }
}
