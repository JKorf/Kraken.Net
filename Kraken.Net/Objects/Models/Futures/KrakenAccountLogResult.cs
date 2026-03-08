namespace Kraken.Net.Objects.Models.Futures
{
    /// <summary>
    /// Account log
    /// </summary>
    [SerializationModel]
    public record KrakenAccountLogResult
    {
        /// <summary>
        /// ["<c>accountUid</c>"] Account id
        /// </summary>
        [JsonPropertyName("accountUid")]
        public string AccountUid { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>logs</c>"] Log entries
        /// </summary>
        [JsonPropertyName("logs")]
        public KrakenAccountLog[] Logs { get; set; } = Array.Empty<KrakenAccountLog>();
    }

    /// <summary>
    /// Log entry
    /// </summary>
    [SerializationModel]
    public record KrakenAccountLog
    {
        /// <summary>
        /// ["<c>asset</c>"] The asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string? Asset { get; set; }
        /// <summary>
        /// ["<c>booking_uid</c>"] Booking id
        /// </summary>
        [JsonPropertyName("booking_uid")]
        public string? BookingUid { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>collateral</c>"] Collateral
        /// </summary>
        [JsonPropertyName("collateral")]
        public string Collateral { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>contract</c>"] Contract
        /// </summary>
        [JsonPropertyName("contract")]
        public string Contract { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>conversion_spread_percentage</c>"] Conversion speed percentage
        /// </summary>
        [JsonPropertyName("conversion_spread_percentage")]
        public decimal? ConversionSpeedPercentage { get; set; }
        /// <summary>
        /// ["<c>date</c>"] Event time
        /// </summary>
        [JsonPropertyName("date")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>execution</c>"] Execution
        /// </summary>
        [JsonPropertyName("execution")]
        public string Execution { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>fee</c>"] Fee paid
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal? Fee { get; set; }
        /// <summary>
        /// ["<c>funding_rate</c>"] Funding rate
        /// </summary>
        [JsonPropertyName("funding_rate")]
        public decimal? FundingRate { get; set; }
        /// <summary>
        /// ["<c>id</c>"] Log id
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }
        /// <summary>
        /// ["<c>info</c>"] Info
        /// </summary>
        [JsonPropertyName("info")]
        public string Info { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>liquidation_fee</c>"] Liquidation fee
        /// </summary>
        [JsonPropertyName("liquidation_fee")]
        public decimal? LiquidationFee { get; set; }
        /// <summary>
        /// ["<c>margin_account</c>"] Margin account
        /// </summary>
        [JsonPropertyName("margin_account")]
        public string? MarginAccount { get; set; }
        /// <summary>
        /// ["<c>mark_price</c>"] Mark price
        /// </summary>
        [JsonPropertyName("mark_price")]
        public decimal? MarkPrice { get; set; }
        /// <summary>
        /// ["<c>new_average_entry_price</c>"] New average entry price
        /// </summary>
        [JsonPropertyName("new_average_entry_price")]
        public decimal? NewAverageEntryPrice { get; set; }
        /// <summary>
        /// ["<c>new_balance</c>"] New balance
        /// </summary>
        [JsonPropertyName("new_balance")]
        public decimal? NewBalance { get; set; }
        /// <summary>
        /// ["<c>old_average_entry_price</c>"] Previous average entry price
        /// </summary>
        [JsonPropertyName("old_average_entry_price")]
        public decimal? OldAverageEntryPrice { get; set; }
        /// <summary>
        /// ["<c>old_balance</c>"] Previous balance
        /// </summary>
        [JsonPropertyName("old_balance")]
        public decimal? OldBalance { get; set; }
        /// <summary>
        /// ["<c>realized_funding</c>"] Realized funding
        /// </summary>
        [JsonPropertyName("realized_funding")]
        public decimal? RealizedFunding { get; set; }
        /// <summary>
        /// ["<c>realized_pnl</c>"] Realized profit and loss
        /// </summary>
        [JsonPropertyName("realized_pnl")]
        public decimal? RealizedProfitAndLoss { get; set; }
        /// <summary>
        /// ["<c>trade_price</c>"] Trade price
        /// </summary>
        [JsonPropertyName("trade_price")]
        public decimal? TradePrice { get; set; }
    }
}
