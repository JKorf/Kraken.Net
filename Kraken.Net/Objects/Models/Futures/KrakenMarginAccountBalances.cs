using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Models.Futures
{
    /// <summary>
    /// Margin account balances
    /// </summary>
    public class KrakenMarginAccountBalances : KrakenBalances
    {
        /// <summary>
        /// Balances
        /// </summary>
        public Dictionary<string, decimal> Balances { get; set; } = new Dictionary<string, decimal>();
        /// <summary>
        /// Currency
        /// </summary>
        public string Currency { get; set; } = string.Empty;
        /// <summary>
        /// Auxiliary account info
        /// </summary>
        public KrakenAuxiliaryAccountInfo? Auxiliary { get; set; }
        /// <summary>
        /// Account's margin requirements.
        /// </summary>
        public KrakenMarginRequirements? MarginRequirements { get; set; }
        /// <summary>
        /// Account's margin trigger estimates.
        /// </summary>
        public KrakenMarginRequirements? TriggerEstimates { get; set; }
    }

    /// <summary>
    /// Margin requirements
    /// </summary>
    public class KrakenMarginRequirements
    {
        /// <summary>
        /// The initial margin requirement of the account.
        /// </summary>
        [JsonProperty("im")]
        public decimal? InitialMargin { get; set; }
        /// <summary>
        /// The liquidation threshold of the account.
        /// </summary>
        [JsonProperty("lt")]
        public decimal? LiquidationThreshold { get; set; }
        /// <summary>
        /// The maintenance margin requirement of the account.
        /// </summary>
        [JsonProperty("mm")]
        public decimal? MaintenanceMargin { get; set; }
        /// <summary>
        /// The termination threshold of the account
        /// </summary>
        [JsonProperty("tt")]
        public decimal? TerminationThreshold { get; set; }

    }

    /// <summary>
    /// Auxiliary account info
    /// </summary>
    public class KrakenAuxiliaryAccountInfo
    {
        /// <summary>
        /// The available funds of the account, in currency.
        /// </summary>
        [JsonProperty("af")]
        public decimal? AvailableFunds { get; set; }
        /// <summary>
        /// Funding
        /// </summary>
        public decimal? Funding { get; set; }
        /// <summary>
        /// The PnL of current open positions of the account, in currency.
        /// </summary>
        [JsonProperty("pnl")]
        public decimal? ProfitAndLoss { get; set; }
        /// <summary>
        /// The portfolio value of the account, in currency.
        /// </summary>
        [JsonProperty("pv")]
        public decimal? PortfolioValue { get; set; }
        /// <summary>
        /// Usd
        /// </summary>
        [JsonProperty("usd")]
        public decimal? Usd { get; set; }
    }
}
