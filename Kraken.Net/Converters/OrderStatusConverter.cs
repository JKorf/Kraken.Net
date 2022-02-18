using System.Collections.Generic;
using CryptoExchange.Net.Converters;
using Kraken.Net.Enums;

namespace Kraken.Net.Converters
{
    internal class OrderStatusConverter: BaseConverter<OrderStatus>
    {
        public OrderStatusConverter() : this(true) { }
        public OrderStatusConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OrderStatus, string>> Mapping => new List<KeyValuePair<OrderStatus, string>>
        {
            new KeyValuePair<OrderStatus, string>(OrderStatus.Open, "open"),
            new KeyValuePair<OrderStatus, string>(OrderStatus.Pending, "pending"),
            new KeyValuePair<OrderStatus, string>(OrderStatus.Closed, "closed"),
            new KeyValuePair<OrderStatus, string>(OrderStatus.Canceled, "canceled"),
            new KeyValuePair<OrderStatus, string>(OrderStatus.Expired, "expired"),
        };
    }
}
