using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Account status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<AccountStatus>))]
    public enum AccountStatus
    {
        /// <summary>
        /// ["<c>active</c>"] Active
        /// </summary>
        [Map("active")]
        Active,
        /// <summary>
        /// ["<c>disabled</c>"] Disabled
        /// </summary>
        [Map("disabled")]
        Disabled,
        /// <summary>
        /// ["<c>closed</c>"] Closed
        /// </summary>
        [Map("closed")]
        Closed,
        /// <summary>
        /// ["<c>unknown</c>"] Unknown
        /// </summary>
        [Map("unknown")]
        Unknown
    }
}
