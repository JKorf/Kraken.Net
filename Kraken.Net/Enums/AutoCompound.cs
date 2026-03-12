using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Auto compound
    /// </summary>
    [JsonConverter(typeof(EnumConverter<AutoCompound>))]
    public enum AutoCompound
    {
        /// <summary>
        /// ["<c>disabled</c>"] Disabled
        /// </summary>
        [Map("disabled")]
        Disabled,
        /// <summary>
        /// ["<c>enabled</c>"] Enabled
        /// </summary>
        [Map("enabled")]
        Enabled,
        /// <summary>
        /// ["<c>optional</c>"] Optional
        /// </summary>
        [Map("optional")]
        Optional
    }
}
