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
        [Map("GTC")]
        GTC,
        /// <summary>
        /// ["<c>IOC</c>"] Immediate Or Cancel
        /// </summary>
        [Map("IOC")]
        IOC,
        /// <summary>
        /// ["<c>GTD</c>"] Good 'Til Date
        /// </summary>
        [Map("GTD")]
        GTD
    }
}
