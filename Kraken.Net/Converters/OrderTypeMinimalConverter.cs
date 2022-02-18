using System.Collections.Generic;
using CryptoExchange.Net.Converters;
using Kraken.Net.Enums;

namespace Kraken.Net.Converters
{
    internal class OrderTypeMinimalConverter: BaseConverter<OrderTypeMinimal>
    {
        public OrderTypeMinimalConverter() : this(true) { }
        public OrderTypeMinimalConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OrderTypeMinimal, string>> Mapping => new List<KeyValuePair<OrderTypeMinimal, string>>
        {
            new KeyValuePair<OrderTypeMinimal, string>(OrderTypeMinimal.Limit, "l"),
            new KeyValuePair<OrderTypeMinimal, string>(OrderTypeMinimal.Market, "m"),
        };
    }
}
