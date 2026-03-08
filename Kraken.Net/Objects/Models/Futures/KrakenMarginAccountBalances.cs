namespace Kraken.Net.Objects.Models.Futures
{
    /// <summary>
    /// Margin account balances
    /// </summary>
    [SerializationModel]
    public record KrakenMarginAccountBalances : KrakenBalances
    {
        /// <summary>
        /// Symbol
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>balances</c>"] Balances
        /// </summary>
        [JsonPropertyName("balances")]
        public Dictionary<string, decimal> Balances { get; set; } = new Dictionary<string, decimal>();
        /// <summary>
        /// ["<c>currency</c>"] Currency
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>auxiliary</c>"] Auxiliary account info
        /// </summary>
        [JsonPropertyName("auxiliary")]
        public KrakenAuxiliaryAccountInfo? Auxiliary { get; set; }
        /// <summary>
        /// ["<c>marginRequirements</c>"] Account's margin requirements.
        /// </summary>
        [JsonPropertyName("marginRequirements")]
        public KrakenMarginRequirements? MarginRequirements { get; set; }
        /// <summary>
        /// ["<c>triggerEstimates</c>"] Account's margin trigger estimates.
        /// </summary>
        [JsonPropertyName("triggerEstimates")]
        public KrakenMarginRequirements? TriggerEstimates { get; set; }
    }

    /// <summary>
    /// Margin requirements
    /// </summary>
    [SerializationModel]
    public record KrakenMarginRequirements
    {
        /// <summary>
        /// ["<c>im</c>"] The initial margin requirement of the account.
        /// </summary>
        [JsonPropertyName("im")]
        public decimal? InitialMargin { get; set; }
        /// <summary>
        /// ["<c>lt</c>"] The liquidation threshold of the account.
        /// </summary>
        [JsonPropertyName("lt")]
        public decimal? LiquidationThreshold { get; set; }
        /// <summary>
        /// ["<c>mm</c>"] The maintenance margin requirement of the account.
        /// </summary>
        [JsonPropertyName("mm")]
        public decimal? MaintenanceMargin { get; set; }
        /// <summary>
        /// ["<c>tt</c>"] The termination threshold of the account
        /// </summary>
        [JsonPropertyName("tt")]
        public decimal? TerminationThreshold { get; set; }

    }

    /// <summary>
    /// Auxiliary account info
    /// </summary>
    [SerializationModel]
    public record KrakenAuxiliaryAccountInfo
    {
        /// <summary>
        /// ["<c>af</c>"] The available funds of the account, in currency.
        /// </summary>
        [JsonPropertyName("af")]
        public decimal? AvailableFunds { get; set; }
        /// <summary>
        /// ["<c>funding</c>"] Funding
        /// </summary>
        [JsonPropertyName("funding")]
        public decimal? Funding { get; set; }
        /// <summary>
        /// ["<c>pnl</c>"] The PnL of current open positions of the account, in currency.
        /// </summary>
        [JsonPropertyName("pnl")]
        public decimal? ProfitAndLoss { get; set; }
        /// <summary>
        /// ["<c>pv</c>"] The portfolio value of the account, in currency.
        /// </summary>
        [JsonPropertyName("pv")]
        public decimal? PortfolioValue { get; set; }
        /// <summary>
        /// ["<c>usd</c>"] Usd
        /// </summary>
        [JsonPropertyName("usd")]
        public decimal? Usd { get; set; }
    }
}
