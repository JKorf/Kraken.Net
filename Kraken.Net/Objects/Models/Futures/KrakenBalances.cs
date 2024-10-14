using Kraken.Net.Converters;

namespace Kraken.Net.Objects.Models.Futures
{
    internal record KrakenBalancesResult : KrakenFuturesResult<KrakenFuturesBalances>
    {
        [JsonPropertyName("accounts")]
        [JsonConverter(typeof(KrakenFuturesBalancesConverter))]
        public override KrakenFuturesBalances Data { get; set; } = null!;
    }

    /// <summary>
    /// Kraken balances info
    /// </summary>
    public record KrakenBalances
    {
        /// <summary>
        /// Type of the balance info
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }

    /// <summary>
    /// Balance info
    /// </summary>
    public record KrakenFuturesBalances
    {
        /// <summary>
        /// Cash account
        /// </summary>
        public KrakenCashBalances CashAccount { get; set; }
        /// <summary>
        /// Multi collateral margin account
        /// </summary>
        public KrakenMultiCollateralMarginBalances MultiCollateralMarginAccount { get; set; }
        /// <summary>
        /// Margin accounts
        /// </summary>
        public IEnumerable<KrakenMarginAccountBalances> MarginAccounts { get; set; }
    }
}
