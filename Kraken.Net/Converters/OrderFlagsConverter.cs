using System.Collections.Generic;
using CryptoExchange.Net.Converters;
using Kraken.Net.Enums;

namespace Kraken.Net.Converters
{
    internal class OrderFlagsConverter : BaseConverter<OrderFlags>
    {
        public OrderFlagsConverter() : this(true) { }
        public OrderFlagsConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OrderFlags, string>> Mapping => new List<KeyValuePair<OrderFlags, string>>
        {
            new KeyValuePair<OrderFlags, string>(OrderFlags.PostOnly, "post"),
            new KeyValuePair<OrderFlags, string>(OrderFlags.FeeCalculationInBaseAsset, "fcib"),
            new KeyValuePair<OrderFlags, string>(OrderFlags.FeeCalculationInQuoteAsset, "fciq"),
            new KeyValuePair<OrderFlags, string>(OrderFlags.NoMarketPriceProtection, "nompp"),
            new KeyValuePair<OrderFlags, string>(OrderFlags.OrderVolumeExpressedInQuoteAsset, "viqc")
        };
    }
}