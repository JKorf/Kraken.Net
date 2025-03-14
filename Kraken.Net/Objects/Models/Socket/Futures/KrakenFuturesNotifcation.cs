using CryptoExchange.Net.Converters.SystemTextJson;
using Kraken.Net.Objects.Models.Futures;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Notification update
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesNotificationUpdate : KrakenFuturesSocketMessage
    {
        /// <summary>
        /// Notifications
        /// </summary>
        [JsonPropertyName("notifications")]
        public KrakenFuturesNotifcation[] Notifications { get; set; } = Array.Empty<KrakenFuturesNotifcation>();
    }

    /// <summary>
    /// Notication
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesNotifcation : KrakenFuturesPlatfromNotification
    {
        /// <summary>
        /// Notification id
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
