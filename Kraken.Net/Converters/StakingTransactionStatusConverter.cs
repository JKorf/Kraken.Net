using System.Collections.Generic;
using CryptoExchange.Net.Converters;
using Kraken.Net.Enums;

namespace Kraken.Net.Converters
{
    internal class StakingTransactionStatusConverter : BaseConverter<StakingTransactionStatus>
    {
        public StakingTransactionStatusConverter() : this(true) { }
        public StakingTransactionStatusConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<StakingTransactionStatus, string>> Mapping => new List<KeyValuePair<StakingTransactionStatus, string>>
        {
            new KeyValuePair<StakingTransactionStatus, string>(StakingTransactionStatus.Failure, "failure"),
            new KeyValuePair<StakingTransactionStatus, string>(StakingTransactionStatus.Initial, "initial"),
            new KeyValuePair<StakingTransactionStatus, string>(StakingTransactionStatus.Pending, "pending"),
            new KeyValuePair<StakingTransactionStatus, string>(StakingTransactionStatus.Settled, "settled"),
            new KeyValuePair<StakingTransactionStatus, string>(StakingTransactionStatus.Success, "success"),
        };
    }
}
