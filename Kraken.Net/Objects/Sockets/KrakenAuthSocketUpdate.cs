//using CryptoExchange.Net.Attributes;
//using CryptoExchange.Net.Converters;

//namespace Kraken.Net.Objects.Sockets
//{
//    [JsonConverter(typeof(ArrayConverter))]
//    internal class KrakenAuthSocketUpdate<T>
//    {
//        [ArrayProperty(0)]
//        [JsonConversion]
//        public T Data { get; set; } = default!;
//        [ArrayProperty(1)]
//        public string ChannelName { get; set; } = null!;
//        [ArrayProperty(2)]
//        [JsonConversion]
//        public KrakenAuthSequence Sequence { get; set; } = null!;
//    }

//    internal class KrakenAuthSequence
//    {
//        [JsonPropertyName("sequence")]
//        public int Sequence { get; set; }
//    }
//}
