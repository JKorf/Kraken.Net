using System.Collections.Generic;
using CryptoExchange.Net.Converters;
using Kraken.Net.Enums;

namespace Kraken.Net.Converters
{
    internal class SystemStatusConverter : BaseConverter<SystemStatus>
    {
        public SystemStatusConverter() : this(true) { }
        public SystemStatusConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<SystemStatus, string>> Mapping => new List<KeyValuePair<SystemStatus, string>>
        {
            new KeyValuePair<SystemStatus, string>(SystemStatus.Online, "online"),
            new KeyValuePair<SystemStatus, string>(SystemStatus.Maintenance, "maintenance"),
            new KeyValuePair<SystemStatus, string>(SystemStatus.CancelOnly, "cancel_only"),
            new KeyValuePair<SystemStatus, string>(SystemStatus.PostOnly, "post_only"),
        };
    }
}
