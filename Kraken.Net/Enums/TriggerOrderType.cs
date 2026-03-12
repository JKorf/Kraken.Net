using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Type of order
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TriggerOrderType>))]
    public enum TriggerOrderType
    {
        /// <summary>
        /// ["<c>TRIGGER_ORDER</c>"] Trigger order
        /// </summary>
        [Map("TRIGGER_ORDER")]
        TriggerOrder,
        /// <summary>
        /// ["<c>ORDER</c>"] Normal order
        /// </summary>
        [Map("ORDER")]
        Order
    }
}
