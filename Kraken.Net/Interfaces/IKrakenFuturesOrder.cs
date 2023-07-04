using Kraken.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Interfaces
{
    public interface IKrakenFuturesOrder
    {
        string? ClientOrderId { get; set; }
        decimal QuantityFilled { get; set; }
        DateTime? LastUpdateTime { get; set; }
        decimal? Price { get; set; }
        string OrderId { get; set; }
        decimal Quantity { get; set; }
        decimal QuantityRemaining { get; set; }
        bool ReduceOnly { get; set; }
        OrderSide Side { get; set; }
        string Symbol { get; set; }
        DateTime Timestamp { get; set; }
    }
}
