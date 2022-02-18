using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace Kraken.Net.Objects.Internal
{
    /// <summary>
    /// Event received from the socket
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [JsonConverter(typeof(ArrayConverter))]
    internal class KrakenSocketEvent<T>
    {
        /// <summary>
        /// Id of the channel
        /// </summary>
        [ArrayProperty(0)]
        public int ChannelId { get; set; }

        /// <summary>
        /// The data
        /// </summary>
        [ArrayProperty(1), JsonConversion]
        public T Data { get; set; } = default!;

        /// <summary>
        /// The topic of the data
        /// </summary>
        [ArrayProperty(2)]
        public string Topic { get; set; } = string.Empty;

        /// <summary>
        /// The symbol the data is for
        /// </summary>
        [ArrayProperty(3)]
        public string Symbol { get; set; } = string.Empty;
    }
}
