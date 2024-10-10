//using CryptoExchange.Net.Attributes;
//using CryptoExchange.Net.Converters;

//namespace Kraken.Net.Objects.Sockets
//{
//    [JsonConverter(typeof(ArrayConverter))]
//    internal class KrakenSocketUpdate<T>
//    {
//        [ArrayProperty(0)]
//        public long ChannelId { get; set; }
//        [ArrayProperty(1)]
//        [JsonConversion]
//        public T Data { get; set; } = default!;
//        [ArrayProperty(2)]
//        public string ChannelName { get; set; } = string.Empty;
//        [ArrayProperty(3)]
//        public string Symbol { get; set; } = string.Empty;
//    }
//}
