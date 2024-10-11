using Kraken.Net.Objects.Models.Futures;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Notification update
    /// </summary>
    public record KrakenFuturesNotificationUpdate : KrakenFuturesSocketMessage
    {
        /// <summary>
        /// Notifications
        /// </summary>
        [JsonPropertyName("notifications")]
        public IEnumerable<KrakenFuturesNotifcation> Notifications { get; set; } = Array.Empty<KrakenFuturesNotifcation>();
    }

    /// <summary>
    /// Notication
    /// </summary>
    public record KrakenFuturesNotifcation : KrakenFuturesPlatfromNotification
    {
        /// <summary>
        /// Notification id
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
