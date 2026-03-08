namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Balance update
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesBalancesUpdate : KrakenFuturesSocketMessage
    {
        /// <summary>
        /// ["<c>account</c>"] Account id
        /// </summary>
        [JsonPropertyName("account")]
        public string Account { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>seq</c>"] Sequence
        /// </summary>
        [JsonPropertyName("seq")]
        public long Sequence { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>holding</c>"] Holdings
        /// </summary>
        [JsonPropertyName("holding")]
        public Dictionary<string, decimal>? Holdings { get; set; }
        /// <summary>
        /// ["<c>futures</c>"] Futures balances
        /// </summary>
        [JsonPropertyName("futures")]
        public Dictionary<string, KrakenFutureBalance>? Futures { get; set; }
        /// <summary>
        /// ["<c>flex_futures</c>"] Flex futures
        /// </summary>
        [JsonPropertyName("flex_futures")]
        public KrakenFlexFutures? FlexFutures { get; set; }
    }

    /// <summary>
    /// Flex futures
    /// </summary>
    [SerializationModel]
    public record KrakenFlexFutures
    {
        /// <summary>
        /// ["<c>currencies</c>"] A map from collateral wallet names to collateral wallet structure
        /// </summary>
        [JsonPropertyName("currencies")]
        public Dictionary<string, KrakenFlexFuturesCurrency>? Currencies { get; set; }
        /// <summary>
        /// ["<c>isolated</c>"] The current margin information for isolated position(s)
        /// </summary>
        [JsonPropertyName("isolated")]
        public Dictionary<string, KrakenFlexIsolatedBalance>? Isolated { get; set; }
        /// <summary>
        /// ["<c>cross</c>"] The current margin information for cross position(s)
        /// </summary>
        [JsonPropertyName("cross")]
        public KrakenFlexCrossBalance? Cross { get; set; }
        /// <summary>
        /// ["<c>balance_value</c>"] The current USD balance of the account
        /// </summary>
        [JsonPropertyName("balance_value")]
        public decimal BalanceValue { get; set; }
        /// <summary>
        /// ["<c>portfolio_value</c>"] The current collateral value with unrealized margin from any open positions
        /// </summary>
        [JsonPropertyName("portfolio_value")]
        public decimal PortfolioValue { get; set; }
        /// <summary>
        /// ["<c>collateral_value</c>"] The current USD balance with haircuts
        /// </summary>
        [JsonPropertyName("collateral_value")]
        public decimal CollateralValue { get; set; }
        /// <summary>
        /// ["<c>initial_margin</c>"] The total initial margin for open positions and open orders
        /// </summary>
        [JsonPropertyName("initial_margin")]
        public decimal InitialMargin { get; set; }
        /// <summary>
        /// ["<c>initial_margin_without_orders</c>"] The total initial margin for open positions
        /// </summary>
        [JsonPropertyName("initial_margin_without_orders")]
        public decimal InitialMarginWithoutOrders { get; set; }
        /// <summary>
        /// ["<c>maintenance_margin</c>"] The total maintenance margin for open positions
        /// </summary>
        [JsonPropertyName("maintenance_margin")]
        public decimal MaintenanceMargin { get; set; }
        /// <summary>
        ///	["<c>pnl</c>"] The total profit and loss for open positions
        /// </summary>
        [JsonPropertyName("pnl")]
        public decimal ProfitAndLoss { get; set; }
        /// <summary>
        ///	["<c>unrealized_funding</c>"] The total unrealized funding for open positions
        /// </summary>
        [JsonPropertyName("unrealized_funding")]
        public decimal UnrealizedFunding { get; set; }
        /// <summary>
        ///	["<c>total_unrealized</c>"] The total unrealized funding and pnl
        /// </summary>
        [JsonPropertyName("total_unrealized")]
        public decimal TotalUnrealized { get; set; }
        /// <summary>
        ///	["<c>total_unrealized_as_margin</c>"] The total unrealized in USD
        /// </summary>
        [JsonPropertyName("total_unrealized_as_margin")]
        public decimal TotalUnrealizedAsMargin { get; set; }
        /// <summary>
        ///	["<c>margin_equity</c>"] The current collateral value and unrealized margin
        /// </summary>
        [JsonPropertyName("margin_equity")]
        public decimal MarginEquity { get; set; }
        /// <summary>
        ///	["<c>available_margin</c>"] The margin equity minus initial margin
        /// </summary>
        [JsonPropertyName("available_margin")]
        public decimal AvailableMargin { get; set; }
    }

    /// <summary>
    /// Cross margin balance
    /// </summary>
    [SerializationModel]
    public record KrakenFlexCrossBalance
    {
        /// <summary>
        /// ["<c>initial_margin</c>"] The total initial margin for open positions and open orders
        /// </summary>
        [JsonPropertyName("initial_margin")]
        public decimal InitialMargin { get; set; }
        /// <summary>
        /// ["<c>initial_margin_without_orders</c>"] The total initial margin for open positions
        /// </summary>
        [JsonPropertyName("initial_margin_without_orders")]
        public decimal InitialMarginWithoutOrders { get; set; }
        /// <summary>
        /// ["<c>maintenance_margin</c>"] The total maintenance margin for open positions
        /// </summary>
        [JsonPropertyName("maintenance_margin")]
        public decimal MaintenanceMargin { get; set; }
        /// <summary>
        ///	["<c>pnl</c>"] The total profit and loss for open positions
        /// </summary>
        [JsonPropertyName("pnl")]
        public decimal ProfitAndLoss { get; set; }
        /// <summary>
        ///	["<c>unrealized_funding</c>"] The total unrealized funding for open positions
        /// </summary>
        [JsonPropertyName("unrealized_funding")]
        public decimal UnrealizedFunding { get; set; }
        /// <summary>
        ///	["<c>total_unrealized</c>"] The total unrealized funding and pnl
        /// </summary>
        [JsonPropertyName("total_unrealized")]
        public decimal TotalUnrealized { get; set; }
        /// <summary>
        ///	["<c>total_unrealized_as_margin</c>"] The total unrealized in USD
        /// </summary>
        [JsonPropertyName("total_unrealized_as_margin")]
        public decimal TotalUnrealizedAsMargin { get; set; }
        /// <summary>
        /// ["<c>balance_value</c>"] The current USD balance of the account
        /// </summary>
        [JsonPropertyName("balance_value")]
        public decimal BalanceValue { get; set; }
        /// <summary>
        /// ["<c>portfolio_value</c>"] The current collateral value with unrealized margin from any open positions
        /// </summary>
        [JsonPropertyName("portfolio_value")]
        public decimal PortfolioValue { get; set; }
        /// <summary>
        /// ["<c>collateral_value</c>"] The current USD balance with haircuts
        /// </summary>
        [JsonPropertyName("collateral_value")]
        public decimal CollateralValue { get; set; }
        /// <summary>
        ///	["<c>margin_equity</c>"] The current collateral value and unrealized margin
        /// </summary>
        [JsonPropertyName("margin_equity")]
        public decimal MarginEquity { get; set; }
        /// <summary>
        ///	["<c>available_margin</c>"] The margin equity minus initial margin
        /// </summary>
        [JsonPropertyName("available_margin")]
        public decimal AvailableMargin { get; set; }
        /// <summary>
        ///	["<c>effective_leverage</c>"] Ratio of position size to margin equity
        /// </summary>
        [JsonPropertyName("effective_leverage")]
        public decimal EffectiveLeverage { get; set; }
    }

    /// <summary>
    /// Isolated margin balance
    /// </summary>
    [SerializationModel]
    public record KrakenFlexIsolatedBalance
    {
        /// <summary>
        /// ["<c>initial_margin</c>"] The total initial margin for open positions and open orders
        /// </summary>
        [JsonPropertyName("initial_margin")]
        public decimal InitialMargin { get; set; }
        /// <summary>
        /// ["<c>initial_margin_without_orders</c>"] The total initial margin for open positions
        /// </summary>
        [JsonPropertyName("initial_margin_without_orders")]
        public decimal InitialMarginWithoutOrders { get; set; }
        /// <summary>
        /// ["<c>maintenance_margin</c>"] The total maintenance margin for open positions
        /// </summary>
        [JsonPropertyName("maintenance_margin")]
        public decimal MaintenanceMargin { get; set; }
        /// <summary>
        ///	["<c>pnl</c>"] The total profit and loss for open positions
        /// </summary>
        [JsonPropertyName("pnl")]
        public decimal ProfitAndLoss { get; set; }
        /// <summary>
        ///	["<c>unrealized_funding</c>"] The total unrealized funding for open positions
        /// </summary>
        [JsonPropertyName("unrealized_funding")]
        public decimal UnrealizedFunding { get; set; }
        /// <summary>
        ///	["<c>total_unrealized</c>"] The total unrealized funding and pnl
        /// </summary>
        [JsonPropertyName("total_unrealized")]
        public decimal TotalUnrealized { get; set; }
        /// <summary>
        ///	["<c>total_unrealized_as_margin</c>"] The total unrealized in USD
        /// </summary>
        [JsonPropertyName("total_unrealized_as_margin")]
        public decimal TotalUnrealizedAsMargin { get; set; }
    }

    /// <summary>
    /// Currency info
    /// </summary>
    [SerializationModel]
    public record KrakenFlexFuturesCurrency
    {
        /// <summary>
        /// ["<c>quantity</c>"] The currency quantity
        /// </summary>
        [JsonPropertyName("quantity")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>value</c>"] The USD value of the currency balance
        /// </summary>
        [JsonPropertyName("value")]
        public decimal Value { get; set; }
        /// <summary>
        /// ["<c>collateral_value</c>"] The current USD balance with haircuts
        /// </summary>
        [JsonPropertyName("collateral_value")]
        public decimal CollateralValue { get; set; }
        /// <summary>
        /// ["<c>available</c>"] The total available margin valued in the wallet currency
        /// </summary>
        [JsonPropertyName("available")]
        public decimal Available { get; set; }
        /// <summary>
        /// ["<c>haircut</c>"] The rate of reduction in the value of a collateral asset that may be used as margin
        /// </summary>
        [JsonPropertyName("haircut")]
        public decimal Haircut { get; set; }
        /// <summary>
        /// ["<c>conversion_spread</c>"] The conversion spread is used to calculate conversion fee for multi-collateral wallets
        /// </summary>
        [JsonPropertyName("conversion_spread")]
        public decimal ConversionSpread { get; set; }
    }

    /// <summary>
    /// Future balance
    /// </summary>
    [SerializationModel]
    public record KrakenFutureBalance
    {
        /// <summary>
        /// ["<c>name</c>"] The name of the account
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>pair</c>"] The wallet currency pair
        /// </summary>
        [JsonPropertyName("pair")]
        public string Pair { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>unit</c>"] The wallet settlement unit
        /// </summary>
        [JsonPropertyName("unit")]
        public string Unit { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>portfolio_value</c>"] The current balance with haircuts and any unrealized margin from open positions in settlement units
        /// </summary>
        [JsonPropertyName("portfolio_value")]
        public decimal PortfolioValue { get; set; }
        /// <summary>
        /// ["<c>balance</c>"] The current balance in settlement units
        /// </summary>
        [JsonPropertyName("balance")]
        public decimal Balance { get; set; }
        /// <summary>
        /// ["<c>maintenance_margin</c>"] The maintenance margin for open positions
        /// </summary>
        [JsonPropertyName("maintenance_margin")]
        public decimal MaintenanceMargin { get; set; }
        /// <summary>
        /// ["<c>initial_margin</c>"] The initial margin for open positions and open orders
        /// </summary>
        [JsonPropertyName("initial_margin")]
        public decimal InitialMargin { get; set; }
        /// <summary>
        /// ["<c>available</c>"] The current portfolio value minus initial margin
        /// </summary>
        [JsonPropertyName("available")]
        public decimal Available { get; set; }
        /// <summary>
        /// ["<c>unrealized_funding</c>"] The total unrealized funding for open positions
        /// </summary>
        [JsonPropertyName("unrealized_funding")]
        public decimal UnrealizedFunding { get; set; }
        /// <summary>
        /// ["<c>pnl</c>"] The total profit and loss for open positions
        /// </summary>
        [JsonPropertyName("pnl")]
        public decimal ProfitAndLoss { get; set; }
    }
}
