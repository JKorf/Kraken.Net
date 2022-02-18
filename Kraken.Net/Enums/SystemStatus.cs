namespace Kraken.Net.Enums
{
    /// <summary>
    /// System status info
    /// </summary>
    public enum SystemStatus
    {
        /// <summary>
        /// Kraken is operating normally. All order types may be submitted and trades can occur.
        /// </summary>
        Online,
        /// <summary>
        /// The exchange is offline. No new orders or cancelations may be submitted.
        /// </summary>
        Maintenance,
        /// <summary>
        /// Resting (open) orders can be canceled but no new orders may be submitted. No trades will occur.
        /// </summary>
        CancelOnly,
        /// <summary>
        /// Only post-only limit orders can be submitted. Existing orders may still be canceled. No trades will occur.
        /// </summary>
        PostOnly
    }
}
