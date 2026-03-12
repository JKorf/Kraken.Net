using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Tick type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TickType>))]
    public enum TickType
    {
        /// <summary>
        /// ["<c>spot</c>"] Spot price
        /// </summary>
        [Map("spot")]
        Spot,
        /// <summary>
        /// ["<c>mark</c>"] Mark price
        /// </summary>
        [Map("mark")]
        Mark,
        /// <summary>
        /// ["<c>trade</c>"] Trade price
        /// </summary>
        [Map("trade")]
        Trade
    }
}
