using System.Collections.Generic;
using CryptoExchange.Net.Converters;
using Kraken.Net.Enums;

namespace Kraken.Net.Converters
{
    internal class OrderTypeConverter: BaseConverter<OrderType>
    {
        public OrderTypeConverter() : this(true) { }
        public OrderTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OrderType, string>> Mapping => new List<KeyValuePair<OrderType, string>>
        {
            new KeyValuePair<OrderType, string>(OrderType.Limit, "limit"),
            new KeyValuePair<OrderType, string>(OrderType.Market, "market"),
            new KeyValuePair<OrderType, string>(OrderType.StopMarket, "stop market"),
            new KeyValuePair<OrderType, string>(OrderType.StopLimit, "stop limit"),
            new KeyValuePair<OrderType, string>(OrderType.StopLoss, "stop-loss"),
            new KeyValuePair<OrderType, string>(OrderType.TakeProfit, "take-profit"),
            new KeyValuePair<OrderType, string>(OrderType.StopLossProfit, "stop-loss-profit"),
            new KeyValuePair<OrderType, string>(OrderType.StopLossProfitLimit, "stop-loss-profit-limit"),
            new KeyValuePair<OrderType, string>(OrderType.StopLossLimit, "stop-loss-limit"),
            new KeyValuePair<OrderType, string>(OrderType.TakeProfitLimit, "take-profit-limit"),
            new KeyValuePair<OrderType, string>(OrderType.TrailingStop, "trailing-stop"),
            new KeyValuePair<OrderType, string>(OrderType.TrailingStopLimit, "trailing-stop-limit"),
            new KeyValuePair<OrderType, string>(OrderType.StopLossAndLimit, "stop-loss-and-limit"),
            new KeyValuePair<OrderType, string>(OrderType.SettlePosition, "settle-position"),
        };
    }
}
