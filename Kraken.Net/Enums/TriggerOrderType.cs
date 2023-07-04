using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Type of order
    /// </summary>
    public enum TriggerOrderType
    {
        /// <summary>
        /// Trigger order
        /// </summary>
        [Map("TRIGGER_ORDER")]
        TriggerOrder,
        /// <summary>
        /// Normal order
        /// </summary>
        [Map("ORDER")]
        Order
    }
}
