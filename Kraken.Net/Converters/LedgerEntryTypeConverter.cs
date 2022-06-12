using System.Collections.Generic;
using CryptoExchange.Net.Converters;
using Kraken.Net.Enums;

namespace Kraken.Net.Converters
{
    internal class LedgerEntryTypeConverter: BaseConverter<LedgerEntryType>
    {
        public LedgerEntryTypeConverter() : this(true) { }
        public LedgerEntryTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<LedgerEntryType, string>> Mapping => new List<KeyValuePair<LedgerEntryType, string>>
        {
            new KeyValuePair<LedgerEntryType, string>(LedgerEntryType.Trade, "trade"),
            new KeyValuePair<LedgerEntryType, string>(LedgerEntryType.Deposit, "deposit"),
            new KeyValuePair<LedgerEntryType, string>(LedgerEntryType.Withdrawal, "withdrawal"),
            new KeyValuePair<LedgerEntryType, string>(LedgerEntryType.Margin, "margin"),
            new KeyValuePair<LedgerEntryType, string>(LedgerEntryType.Adjustment, "adjustment"),
            new KeyValuePair<LedgerEntryType, string>(LedgerEntryType.Transfer, "transfer"),
            new KeyValuePair<LedgerEntryType, string>(LedgerEntryType.Rollover, "rollover"),
            new KeyValuePair<LedgerEntryType, string>(LedgerEntryType.Spend, "spend"),
            new KeyValuePair<LedgerEntryType, string>(LedgerEntryType.Receive, "receive"),
            new KeyValuePair<LedgerEntryType, string>(LedgerEntryType.Settled, "settled"),
            new KeyValuePair<LedgerEntryType, string>(LedgerEntryType.Staking, "staking"),
        };
    }
}
