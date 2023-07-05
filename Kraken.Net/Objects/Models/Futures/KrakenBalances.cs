using Kraken.Net.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Futures
{
    internal class KrakenBalancesResult : KrakenFuturesResult<Dictionary<string, KrakenBalances>>
    {
        [JsonProperty("accounts")]
        public override Dictionary<string, KrakenBalances> Data { get; set; } = new Dictionary<string, KrakenBalances>();
    }

    /// <summary>
    /// Kraken balances info
    /// </summary>
    [JsonConverter(typeof(KrakenFuturesBalancesConverter))]
    public class KrakenBalances
    {
        /// <summary>
        /// Type of the balance info
        /// </summary>
        public string Type { get; set; } = string.Empty;
    }
}
