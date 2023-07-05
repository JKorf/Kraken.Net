using Kraken.Net.Enums;
using System;

namespace Kraken.Net.Interfaces
{
    /// <summary>
    /// Order info
    /// </summary>
    public interface IKrakenFuturesOrder
    {
        /// <summary>
        /// Client order id
        /// </summary>
        string? ClientOrderId { get; set; }
        /// <summary>
        /// Quantity filled
        /// </summary>
        decimal QuantityFilled { get; set; }
        /// <summary>
        /// Last update time
        /// </summary>
        DateTime? LastUpdateTime { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        decimal? Price { get; set; }
        /// <summary>
        /// Order id
        /// </summary>
        string OrderId { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        decimal Quantity { get; set; }
        /// <summary>
        /// Quantity remaining
        /// </summary>
        decimal QuantityRemaining { get; set; }
        /// <summary>
        /// Reduce only
        /// </summary>
        bool ReduceOnly { get; set; }
        /// <summary>
        /// Order side
        /// </summary>
        OrderSide Side { get; set; }
        /// <summary>
        /// Symbol
        /// </summary>
        string Symbol { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        DateTime Timestamp { get; set; }
    }
}
