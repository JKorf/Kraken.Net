using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
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
        /// Kraken is operating normally. All order types may be submitted and trades can occur.
        /// </summary>
        [Map("online")]
        Online,
        /// <summary>
        /// The exchange is offline. No new orders or cancelations may be submitted.
        /// </summary>
        [Map("maintenance")]
        Maintenance,
        /// <summary>
        /// Resting (open) orders can be canceled but no new orders may be submitted. No trades will occur.
        /// </summary>
        [Map("cancel_only")]
        CancelOnly,
        /// <summary>
        /// Only post-only limit orders can be submitted. Existing orders may still be canceled. No trades will occur.
        /// </summary>
        [Map("post_only")]
        PostOnly
    }
}
