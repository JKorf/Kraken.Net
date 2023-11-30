using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Sockets
{
    [JsonConverter(typeof(ArrayConverter))]
    internal class KrakenAuthSocketUpdate<T>
    {
        [ArrayProperty(0)]
        [JsonConversion]
        public T Data { get; set; } = default!;
        [ArrayProperty(1)]
        public string ChannelName { get; set; } = null!;
        [ArrayProperty(2)]
        [JsonConversion]
        public KrakenAuthSequence Sequence { get; set; } = null!;
    }

    internal class KrakenAuthSequence
    {
        [JsonProperty("sequence")]
        public int Sequence { get; set; }
    }
}
