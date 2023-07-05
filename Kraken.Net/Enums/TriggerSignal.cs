using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Trigger signal
    /// </summary>
    public enum TriggerSignal
    {
        /// <summary>
        /// Mark price
        /// </summary>
        [Map("mark", "MARK_PRICE")]
        Mark,
        /// <summary>
        /// Index price
        /// </summary>
        [Map("index")]
        Index,
        /// <summary>
        /// Index price
        /// </summary>
        [Map("spot", "SPOT_PRICE")]
        Spot,
        /// <summary>
        /// Last trade price
        /// </summary>
        [Map("last", "LAST_PRICE")]
        Last
    }
}
