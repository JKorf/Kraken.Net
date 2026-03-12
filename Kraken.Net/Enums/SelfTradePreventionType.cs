using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Self trade prevention type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<SelfTradePreventionType>))]
    public enum SelfTradePreventionType
    {
        /// <summary>
        /// ["<c>cancel-newest</c>"] Cancel newest order
        /// </summary>
        [Map("cancel-newest")]
        CancelNewest,
        /// <summary>
        /// ["<c>cancel-oldest</c>"] Cancel oldest order
        /// </summary>
        [Map("cancel-oldest")]
        CancelOldest,
        /// <summary>
        /// ["<c>cancel-both</c>"] Cancel both orders
        /// </summary>
        [Map("cancel-both")]
        CancelBoth
    }
}
