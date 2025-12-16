using Kraken.Net.Converters;

namespace Kraken.Net.Objects.Models.Futures
{
    [SerializationModel]
    internal record KrakenBalancesResult : KrakenFuturesResult<KrakenFuturesBalances>
    {
        [JsonPropertyName("accounts")]
        [JsonConverter(typeof(KrakenFuturesBalancesConverter))]
        public override KrakenFuturesBalances Data { get; set; } = null!;
    }

    /// <summary>
    /// Kraken balances info
    /// </summary>
    [SerializationModel]
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
    [SerializationModel]
    public record KrakenFuturesBalances
    {
        /// <summary>
        /// Cash account
        /// </summary>
        public KrakenCashBalances CashAccount { get; set; } = null!;
        /// <summary>
        /// Multi collateral margin account
        /// </summary>
        public KrakenMultiCollateralMarginBalances MultiCollateralMarginAccount { get; set; } = null!;
        /// <summary>
        /// Margin accounts
        /// </summary>
        public KrakenMarginAccountBalances[] MarginAccounts { get; set; } = [];
    }
}
