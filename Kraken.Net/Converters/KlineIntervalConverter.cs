using System.Collections.Generic;
using CryptoExchange.Net.Converters;
using Kraken.Net.Enums;

namespace Kraken.Net.Converters
{
    internal class KlineIntervalConverter: BaseConverter<KlineInterval>
    {
        public KlineIntervalConverter() : this(true) { }
        public KlineIntervalConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<KlineInterval, string>> Mapping => new List<KeyValuePair<KlineInterval, string>>
        {
            new KeyValuePair<KlineInterval, string>(KlineInterval.OneMinute, "1"),
            new KeyValuePair<KlineInterval, string>(KlineInterval.FiveMinutes, "5"),
            new KeyValuePair<KlineInterval, string>(KlineInterval.FifteenMinutes, "15"),
            new KeyValuePair<KlineInterval, string>(KlineInterval.ThirtyMinutes, "30"),
            new KeyValuePair<KlineInterval, string>(KlineInterval.OneHour, "60"),
            new KeyValuePair<KlineInterval, string>(KlineInterval.FourHour, "240"),
            new KeyValuePair<KlineInterval, string>(KlineInterval.OneDay, "1440"),
            new KeyValuePair<KlineInterval, string>(KlineInterval.OneWeek, "10080"),
            new KeyValuePair<KlineInterval, string>(KlineInterval.FifteenDays, "21600"),
        };
    }
}
