namespace Kraken.Net.Enums
{
    /// <summary>
    /// Ticker update trigger event
    /// </summary>
    public enum TriggerEvent
    {
        /// <summary>
        /// Send update when a trade is done
        /// </summary>
        Trade,
        /// <summary>
        /// Send update when the best bid or ask price/quantity changes
        /// </summary>
        BestOfferChange
    }
}
