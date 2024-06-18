using Kraken.Net.Objects.Models.Futures;
using System;
using System.Collections.Generic;

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
        public int Id { get; set; }
    }
}
