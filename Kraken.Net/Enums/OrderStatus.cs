namespace Kraken.Net.Enums
{
    /// <summary>
    /// Status of an order
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// Pending
        /// </summary>
        Pending,
        /// <summary>
        /// Active, not (fully) filled
        /// </summary>
        Open,
        /// <summary>
        /// Fully filled
        /// </summary>
        Closed,
        /// <summary>
        /// Canceled
        /// </summary>
        Canceled,
        /// <summary>
        /// Expired
        /// </summary>
        Expired
    }
}
