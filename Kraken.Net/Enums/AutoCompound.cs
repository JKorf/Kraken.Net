using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Auto compound
    /// </summary>
    public enum AutoCompound
    {
        /// <summary>
        /// Disabled
        /// </summary>
        [Map("disabled")]
        Disabled,
        /// <summary>
        /// Enabled
        /// </summary>
        [Map("enabled")]
        Enabled,
        /// <summary>
        /// Optional
        /// </summary>
        [Map("optional")]
        Optional
    }
}
