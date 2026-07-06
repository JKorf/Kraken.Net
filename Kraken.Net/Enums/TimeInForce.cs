using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Time In Force
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TimeInForce>))]
    public enum TimeInForce
    {
        /// <summary>
        /// ["<c>GTC</c>"] Good 'Til Cancelled
        /// </summary>
        [Map("GTC", "gtc")]
        GTC,
        /// <summary>
        /// ["<c>IOC</c>"] Immediate Or Cancel
        /// </summary>
        [Map("IOC", "ioc")]
        IOC,
        /// <summary>
        /// ["<c>GTD</c>"] Good 'Til Date
        /// </summary>
        [Map("GTD", "gtd")]
        GTD,
        /// <summary>
        /// ["<c>FOK</c>"] Fill or kill
        /// </summary>
        [Map("FOK", "fok")]
        FOK
    }
}
