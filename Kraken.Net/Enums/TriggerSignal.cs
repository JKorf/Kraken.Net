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
        [Map("mark")]
        Mark,
        /// <summary>
        /// Index price
        /// </summary>
        [Map("index")]
        Index,
        /// <summary>
        /// Index price
        /// </summary>
        [Map("spot")]
        Spot,
        /// <summary>
        /// Last trade price
        /// </summary>
        [Map("last")]
        Last
    }
}
