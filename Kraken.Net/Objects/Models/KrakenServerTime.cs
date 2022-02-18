using System;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace Kraken.Net.Objects.Models
{
    internal class KrakenServerTime
    {
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime UnixTime { get; set; }
        [JsonProperty("rfc1123")]
        public string RfcTime { get; set; } = string.Empty;
    }
}
