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
        /// Good 'Til Cancelled
        /// </summary>
        [Map("GTC")]
        GTC,
        /// <summary>
        /// Immediate Or Cancel
        /// </summary>
        [Map("IOC")]
        IOC,
        /// <summary>
        /// Good 'Til Date
        /// </summary>
        [Map("GTD")]
        GTD
    }
}
