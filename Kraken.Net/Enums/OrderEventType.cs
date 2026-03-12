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
        /// ["<c>pending_new</c>"] Order request has been received and validated but the order is not live yet.
        /// </summary>
        [Map("pending_new")]
        PendingNew,
        /// <summary>
        /// ["<c>new</c>"] Order has been created and is live in the engine.
        /// </summary>
        [Map("new")]
        New,
        /// <summary>
        /// ["<c>trade</c>"] The order has received a fill.
        /// </summary>
        [Map("trade")]
        Trade,
        /// <summary>
        /// ["<c>filled</c>"] The order has been fully filled.
        /// </summary>
        [Map("filled")]
        Filled,
        /// <summary>
        /// ["<c>canceled</c>"] The order has been cancelled.
        /// </summary>
        [Map("canceled")]
        Canceled,
        /// <summary>
        /// ["<c>expired</c>"] The order has expired.
        /// </summary>
        [Map("expired")]
        Expired,
        /// <summary>
        /// ["<c>amended</c>"] There is a user initiated amend on the order, i.e. limit price change.
        /// </summary>
        [Map("amended")]
        Amended,
        /// <summary>
        /// ["<c>restated</c>"] There is a engine initiated amend on the order for maintenance of position or book, see reason field, i.e. reduce non-tradable liquidity.
        /// </summary>
        [Map("restated")]
        Restated,
        /// <summary>
        /// ["<c>status</c>"] The order has a status update, i.e. trigger price has been updated.
        /// </summary>
        [Map("status")]
        Status,
    }

}
