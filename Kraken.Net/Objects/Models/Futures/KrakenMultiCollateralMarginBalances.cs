using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Futures
{
    /// <summary>
    /// Multi-collateral margin account balances
    /// </summary>
    public class KrakenMultiCollateralMarginBalances : KrakenBalances
    {
        /// <summary>
        /// Total initial margin held for open positions (USD).
        /// </summary>
        public decimal InitialMargin { get; set; }
        /// <summary>
        /// Total initial margin held for open positions and open orders (USD).
        /// </summary>
        public decimal InitialMarginWithOrders { get; set; }
        /// <summary>
        /// Total maintenance margin held for open positions (USD).
        /// </summary>
        public decimal MaintenanceMargin { get; set; }
        /// <summary>
        /// USD value of all collateral in multi-collateral wallet.
        /// </summary>
        public decimal BalanceValue { get; set; }
        /// <summary>
        /// Balance value plus unrealised PnL in USD.
        /// </summary>
        public decimal PortfolioValue { get; set; }
        /// <summary>
        /// USD value of balances in account usable for margin (Balance Value * Haircut).
        /// </summary>
        public decimal CollateralValue { get; set; }
        /// <summary>
        /// Unrealised PnL in USD.
        /// </summary>
        [JsonProperty("pnl")]
        public decimal ProfitAndLoss { get; set; }
        /// <summary>
        /// Unrealised funding from funding rate (USD).
        /// </summary>
        public decimal UnrealizedFunding { get; set; }
        /// <summary>
        /// Total USD value of unrealised funding and unrealised PnL.
        /// </summary>
        public decimal TotalUnrealized { get; set; }
        /// <summary>
        /// Unrealised pnl and unrealised funding that is usable as margin [(Unrealised Profit/Loss + Unrealised Funding Rate) * Haircut - Conversion Fee].
        /// </summary>
        public decimal TotalUnrealizedAsMargin { get; set; }
        /// <summary>
        /// Margin Equity - Total Initial Margin.
        /// </summary>
        public decimal AvailableMargin { get; set; }
        /// <summary>
        /// [Balance Value in USD * (1-Haircut)] + (Total Unrealised Profit/Loss as Margin in USD)
        /// </summary>
        public decimal MarginEquity { get; set; }
        /// <summary>
        /// Flex currencies
        /// </summary>
        public Dictionary<string, KrakenFlexCurrencySummary> Currencies { get; set; } = new Dictionary<string, KrakenFlexCurrencySummary>();
    }

    /// <summary>
    /// Flex currency info
    /// </summary>
    public class KrakenFlexCurrencySummary
    {
        /// <summary>
        /// Margin (in base currency) available for trading.
        /// </summary>
        public decimal Available { get; set; }
        /// <summary>
        /// USD value of the asset usable for margin (Asset Value * Haircut).
        /// </summary>
        public decimal Collateral { get; set; }
        /// <summary>
        /// Quantity of asset.
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// USD value of asset.
        /// </summary>
        public decimal Value { get; set; }
    }
}
