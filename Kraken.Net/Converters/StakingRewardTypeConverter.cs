using System.Collections.Generic;
using CryptoExchange.Net.Converters;
using Kraken.Net.Enums;

namespace Kraken.Net.Converters
{
    internal class StakingRewardTypeConverter : BaseConverter<StakingRewardType>
    {
        public StakingRewardTypeConverter() : this(true) { }
        public StakingRewardTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<StakingRewardType, string>> Mapping => new List<KeyValuePair<StakingRewardType, string>>
        {
            new KeyValuePair<StakingRewardType, string>(StakingRewardType.Percentage, "percentage")
        };
    }
}
