using Kraken.Net.Converters;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Futures
{
    internal record KrakenBalancesResult : KrakenFuturesResult<Dictionary<string, KrakenBalances>>
    {
        [JsonPropertyName("accounts")]
        public override Dictionary<string, KrakenBalances> Data { get; set; } = new Dictionary<string, KrakenBalances>();
    }

    /// <summary>
    /// Kraken balances info
    /// </summary>
    [JsonConverter(typeof(KrakenFuturesBalancesConverter))]
    public record KrakenBalances
    {
        /// <summary>
        /// Type of the balance info
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }
}
