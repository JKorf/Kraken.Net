namespace Kraken.Net.Objects.Models.Futures
{
    /// <summary>
    /// Margin account balances
    /// </summary>
    public record KrakenMarginAccountBalances : KrakenBalances
    {
        /// <summary>
        /// Symbol
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Balances
        /// </summary>
        [JsonPropertyName("balances")]
        public Dictionary<string, decimal> Balances { get; set; } = new Dictionary<string, decimal>();
        /// <summary>
        /// Currency
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;
        /// <summary>
        /// Auxiliary account info
        /// </summary>
        [JsonPropertyName("auxiliary")]
        public KrakenAuxiliaryAccountInfo? Auxiliary { get; set; }
        /// <summary>
        /// Account's margin requirements.
        /// </summary>
        [JsonPropertyName("marginRequirements")]
        public KrakenMarginRequirements? MarginRequirements { get; set; }
        /// <summary>
        /// Account's margin trigger estimates.
        /// </summary>
        [JsonPropertyName("triggerEstimates")]
        public KrakenMarginRequirements? TriggerEstimates { get; set; }
    }

    /// <summary>
    /// Margin requirements
    /// </summary>
    public record KrakenMarginRequirements
    {
        /// <summary>
        /// The initial margin requirement of the account.
        /// </summary>
        [JsonPropertyName("im")]
        public decimal? InitialMargin { get; set; }
        /// <summary>
        /// The liquidation threshold of the account.
        /// </summary>
        [JsonPropertyName("lt")]
        public decimal? LiquidationThreshold { get; set; }
        /// <summary>
        /// The maintenance margin requirement of the account.
        /// </summary>
        [JsonPropertyName("mm")]
        public decimal? MaintenanceMargin { get; set; }
        /// <summary>
        /// The termination threshold of the account
        /// </summary>
        [JsonPropertyName("tt")]
        public decimal? TerminationThreshold { get; set; }

    }

    /// <summary>
    /// Auxiliary account info
    /// </summary>
    public record KrakenAuxiliaryAccountInfo
    {
        /// <summary>
        /// The available funds of the account, in currency.
        /// </summary>
        [JsonPropertyName("af")]
        public decimal? AvailableFunds { get; set; }
        /// <summary>
        /// Funding
        /// </summary>
        [JsonPropertyName("funding")]
        public decimal? Funding { get; set; }
        /// <summary>
        /// The PnL of current open positions of the account, in currency.
        /// </summary>
        [JsonPropertyName("pnl")]
        public decimal? ProfitAndLoss { get; set; }
        /// <summary>
        /// The portfolio value of the account, in currency.
        /// </summary>
        [JsonPropertyName("pv")]
        public decimal? PortfolioValue { get; set; }
        /// <summary>
        /// Usd
        /// </summary>
        [JsonPropertyName("usd")]
        public decimal? Usd { get; set; }
    }
}
