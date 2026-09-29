using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>Determines how the size of an edited Futures order is interpreted.</summary>
    [JsonConverter(typeof(EnumConverter<FuturesQuantityMode>))]
    public enum FuturesQuantityMode
    {
        /// <summary>["<c>ABSOLUTE</c>"] Total quantity including past fills.</summary>
        [Map("ABSOLUTE")]
        Absolute,
        /// <summary>["<c>RELATIVE</c>"] Remaining open quantity, excluding past fills.</summary>
        [Map("RELATIVE")]
        Relative
    }
}
