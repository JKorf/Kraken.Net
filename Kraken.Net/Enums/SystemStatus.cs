using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// System status info
    /// </summary>
    [JsonConverter(typeof(EnumConverter<SystemStatus>))]
    public enum SystemStatus
    {
        /// <summary>
        /// ["<c>online</c>"] Kraken is operating normally. All order types may be submitted and trades can occur.
        /// </summary>
        [Map("online")]
        Online,
        /// <summary>
        /// ["<c>maintenance</c>"] The exchange is offline. No new orders or cancelations may be submitted.
        /// </summary>
        [Map("maintenance")]
        Maintenance,
        /// <summary>
        /// ["<c>cancel_only</c>"] Resting (open) orders can be canceled but no new orders may be submitted. No trades will occur.
        /// </summary>
        [Map("cancel_only")]
        CancelOnly,
        /// <summary>
        /// ["<c>post_only</c>"] Only post-only limit orders can be submitted. Existing orders may still be canceled. No trades will occur.
        /// </summary>
        [Map("post_only")]
        PostOnly
    }
}
