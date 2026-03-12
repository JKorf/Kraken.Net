using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Trigger signal
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TriggerSignal>))]
    public enum TriggerSignal
    {
        /// <summary>
        /// ["<c>mark</c>"] Mark price
        /// </summary>
        [Map("mark", "MARK_PRICE")]
        Mark,
        /// <summary>
        /// ["<c>index</c>"] Index price
        /// </summary>
        [Map("index")]
        Index,
        /// <summary>
        /// ["<c>spot</c>"] Index price
        /// </summary>
        [Map("spot", "SPOT_PRICE")]
        Spot,
        /// <summary>
        /// ["<c>last</c>"] Last trade price
        /// </summary>
        [Map("last", "LAST_PRICE")]
        Last
    }
}
