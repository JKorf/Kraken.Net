using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Order event type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderEventType>))]
    public enum OrderEventType
    {
        /// <summary>
        /// Order request has been received and validated but the order is not live yet.
        /// </summary>
        [Map("pending_new")]
        PendingNew,
        /// <summary>
        /// Order has been created and is live in the engine.
        /// </summary>
        [Map("new")]
        New,
        /// <summary>
        /// The order has received a fill.
        /// </summary>
        [Map("trade")]
        Trade,
        /// <summary>
        /// The order has been fully filled.
        /// </summary>
        [Map("filled")]
        Filled,
        /// <summary>
        /// The order has been cancelled.
        /// </summary>
        [Map("canceled")]
        Canceled,
        /// <summary>
        /// The order has expired.
        /// </summary>
        [Map("expired")]
        Expired,
        /// <summary>
        /// There is a user initiated amend on the order, i.e. limit price change.
        /// </summary>
        [Map("amended")]
        Amended,
        /// <summary>
        /// There is a engine initiated amend on the order for maintenance of position or book, see reason field, i.e. reduce non-tradable liquidity.
        /// </summary>
        [Map("restated")]
        Restated,
        /// <summary>
        /// The order has a status update, i.e. trigger price has been updated.
        /// </summary>
        [Map("status")]
        Status,
    }

}
