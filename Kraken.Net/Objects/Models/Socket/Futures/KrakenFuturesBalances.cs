using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Balance update
    /// </summary>
    public class KrakenFuturesBalancesUpdate : KrakenFuturesSocketMessage
    {
        /// <summary>
        /// Account id
        /// </summary>
        public string Account { get; set; } = string.Empty;
        /// <summary>
        /// Sequence
        /// </summary>
        [JsonProperty("seq")]
        public long Sequence { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Holdings
        /// </summary>
        public Dictionary<string, decimal>? Holdings { get; set; }
        /// <summary>
        /// Futures balances
        /// </summary>
        public Dictionary<string, KrakenFutureBalance>? Futures { get; set; }
        /// <summary>
        /// Flex futures
        /// </summary>
        [JsonProperty("flex_futures")]
        public KrakenFlexFutures? FlexFutures { get; set; }
    }

    /// <summary>
    /// Flex futures
    /// </summary>
    public class KrakenFlexFutures
    {
        /// <summary>
        /// A map from collateral wallet names to collateral wallet structure
        /// </summary>
        public Dictionary<string, KrakenFlexFuturesCurrency>? Currencies { get; set; }
        /// <summary>
        /// The current margin information for isolated position(s)
        /// </summary>
        public Dictionary<string, KrakenFlexIsolatedBalance>? Isolated { get; set; }
        /// <summary>
        /// The current margin information for cross position(s)
        /// </summary>
        public KrakenFlexCrossBalance? Cross { get; set; }
        /// <summary>
        /// The current USD balance of the account
        /// </summary>
        [JsonProperty("balance_value")]
        public decimal BalanceValue { get; set; }
        /// <summary>
        /// The current collateral value with unrealized margin from any open positions
        /// </summary>
        [JsonProperty("portfolio_value")]
        public decimal PortfolioValue { get; set; }
        /// <summary>
        /// The current USD balance with haircuts
        /// </summary>
        [JsonProperty("collateral_value")]
        public decimal CollateralValue { get; set; }
        /// <summary>
        /// The total initial margin for open positions and open orders
        /// </summary>
        [JsonProperty("initial_margin")]
        public decimal InitialMargin { get; set; }
        /// <summary>
        /// The total initial margin for open positions
        /// </summary>
        [JsonProperty("initial_margin_without_orders")]
        public decimal InitialMarginWithoutOrders { get; set; }
        /// <summary>
        /// The total maintenance margin for open positions
        /// </summary>
        [JsonProperty("maintenance_margin")]
        public decimal MaintenanceMargin { get; set; }
        /// <summary>
        ///	The total profit and loss for open positions
        /// </summary>
        [JsonProperty("pnl")]
        public decimal ProfitAndLoss { get; set; }
        /// <summary>
        ///	The total unrealized funding for open positions
        /// </summary>
        [JsonProperty("unrealized_funding")]
        public decimal UnrealizedFunding { get; set; }
        /// <summary>
        ///	The total unrealized funding and pnl
        /// </summary>
        [JsonProperty("total_unrealized")]
        public decimal TotalUnrealized { get; set; }
        /// <summary>
        ///	The total unrealized in USD
        /// </summary>
        [JsonProperty("total_unrealized_as_margin")]
        public decimal TotalUnrealizedAsMargin { get; set; }
        /// <summary>
        ///	The current collateral value and unrealized margin
        /// </summary>
        [JsonProperty("margin_equity")]
        public decimal MarginEquity { get; set; }
        /// <summary>
        ///	The margin equity minus initial margin
        /// </summary>
        [JsonProperty("available_margin")]
        public decimal AvailableMargin { get; set; }
    }

    /// <summary>
    /// Cross margin balance
    /// </summary>
    public class KrakenFlexCrossBalance
    {
        /// <summary>
        /// The total initial margin for open positions and open orders
        /// </summary>
        [JsonProperty("initial_margin")]
        public decimal InitialMargin { get; set; }
        /// <summary>
        /// The total initial margin for open positions
        /// </summary>
        [JsonProperty("initial_margin_without_orders")]
        public decimal InitialMarginWithoutOrders { get; set; }
        /// <summary>
        /// The total maintenance margin for open positions
        /// </summary>
        [JsonProperty("maintenance_margin")]
        public decimal MaintenanceMargin { get; set; }
        /// <summary>
        ///	The total profit and loss for open positions
        /// </summary>
        [JsonProperty("pnl")]
        public decimal ProfitAndLoss { get; set; }
        /// <summary>
        ///	The total unrealized funding for open positions
        /// </summary>
        [JsonProperty("unrealized_funding")]
        public decimal UnrealizedFunding { get; set; }
        /// <summary>
        ///	The total unrealized funding and pnl
        /// </summary>
        [JsonProperty("total_unrealized")]
        public decimal TotalUnrealized { get; set; }
        /// <summary>
        ///	The total unrealized in USD
        /// </summary>
        [JsonProperty("total_unrealized_as_margin")]
        public decimal TotalUnrealizedAsMargin { get; set; }
        /// <summary>
        /// The current USD balance of the account
        /// </summary>
        [JsonProperty("balance_value")]
        public decimal BalanceValue { get; set; }
        /// <summary>
        /// The current collateral value with unrealized margin from any open positions
        /// </summary>
        [JsonProperty("portfolio_value")]
        public decimal PortfolioValue { get; set; }
        /// <summary>
        /// The current USD balance with haircuts
        /// </summary>
        [JsonProperty("collateral_value")]
        public decimal CollateralValue { get; set; }
        /// <summary>
        ///	The current collateral value and unrealized margin
        /// </summary>
        [JsonProperty("margin_equity")]
        public decimal MarginEquity { get; set; }
        /// <summary>
        ///	The margin equity minus initial margin
        /// </summary>
        [JsonProperty("available_margin")]
        public decimal AvailableMargin { get; set; }
        /// <summary>
        ///	Ratio of position size to margin equity
        /// </summary>
        [JsonProperty("effective_leverage")]
        public decimal EffectiveLeverage { get; set; }
    }

    /// <summary>
    /// Isolated margin balance
    /// </summary>
    public class KrakenFlexIsolatedBalance
    {
        /// <summary>
        /// The total initial margin for open positions and open orders
        /// </summary>
        [JsonProperty("initial_margin")]
        public decimal InitialMargin { get; set; }
        /// <summary>
        /// The total initial margin for open positions
        /// </summary>
        [JsonProperty("initial_margin_without_orders")]
        public decimal InitialMarginWithoutOrders { get; set; }
        /// <summary>
        /// The total maintenance margin for open positions
        /// </summary>
        [JsonProperty("maintenance_margin")]
        public decimal MaintenanceMargin { get; set; }
        /// <summary>
        ///	The total profit and loss for open positions
        /// </summary>
        [JsonProperty("pnl")]
        public decimal ProfitAndLoss { get; set; }
        /// <summary>
        ///	The total unrealized funding for open positions
        /// </summary>
        [JsonProperty("unrealized_funding")]
        public decimal UnrealizedFunding { get; set; }
        /// <summary>
        ///	The total unrealized funding and pnl
        /// </summary>
        [JsonProperty("total_unrealized")]
        public decimal TotalUnrealized { get; set; }
        /// <summary>
        ///	The total unrealized in USD
        /// </summary>
        [JsonProperty("total_unrealized_as_margin")]
        public decimal TotalUnrealizedAsMargin { get; set; }
    }

    /// <summary>
    /// Currency info
    /// </summary>
    public class KrakenFlexFuturesCurrency
    {
        /// <summary>
        /// The currency quantity
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// The USD value of the currency balance
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// The current USD balance with haircuts
        /// </summary>
        [JsonProperty("collateral_value")]
        public decimal CollateralValue { get; set; }
        /// <summary>
        /// The total available margin valued in the wallet currency
        /// </summary>
        public decimal Available { get; set; }
        /// <summary>
        /// The rate of reduction in the value of a collateral asset that may be used as margin
        /// </summary>
        public decimal Haircut { get; set; }
        /// <summary>
        /// The conversion spread is used to calculate conversion fee for multi-collateral wallets
        /// </summary>
        [JsonProperty("conversion_spread")]
        public decimal ConversionSpread { get; set; }
    }

    /// <summary>
    /// Future balance
    /// </summary>
    public class KrakenFutureBalance
    {
        /// <summary>
        /// The name of the account
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// The wallet currency pair
        /// </summary>
        public string Pair { get; set; } = string.Empty;
        /// <summary>
        /// The wallet settlement unit
        /// </summary>
        public string Unit { get; set; } = string.Empty;
        /// <summary>
        /// The current balance with haircuts and any unrealized margin from open positions in settlement units
        /// </summary>
        [JsonProperty("portfolio_value")]
        public decimal PortfolioValue { get; set; }
        /// <summary>
        /// The current balance in settlement units
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// The maintenance margin for open positions
        /// </summary>
        [JsonProperty("maintenance_margin")]
        public decimal MaintenanceMargin { get; set; }
        /// <summary>
        /// The initial margin for open positions and open orders
        /// </summary>
        [JsonProperty("initial_margin")]
        public decimal InitialMargin { get; set; }
        /// <summary>
        /// The current portfolio value minus initial margin
        /// </summary>
        public decimal Available { get; set; }
        /// <summary>
        /// The total unrealized funding for open positions
        /// </summary>
        [JsonProperty("unrealized_funding")]
        public decimal UnrealizedFunding { get; set; }
        /// <summary>
        /// The total profit and loss for open positions
        /// </summary>
        [JsonProperty("pnl")]
        public decimal ProfitAndLoss { get; set; }
    }
}
