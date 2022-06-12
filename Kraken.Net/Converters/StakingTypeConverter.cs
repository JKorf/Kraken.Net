using System.Collections.Generic;
using CryptoExchange.Net.Converters;
using Kraken.Net.Enums;

namespace Kraken.Net.Converters
{
    internal class StakingTypeConverter : BaseConverter<StakingType>
    {
        public StakingTypeConverter() : this(true) { }
        public StakingTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<StakingType, string>> Mapping => new List<KeyValuePair<StakingType, string>>
        {
            new KeyValuePair<StakingType, string>(StakingType.Bonding, "bonding"),
            new KeyValuePair<StakingType, string>(StakingType.Reward, "reward"),
            new KeyValuePair<StakingType, string>(StakingType.Unbonding, "unbonding"),
        };
    }
}
