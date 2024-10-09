namespace Kraken.Net.Objects.Models.Futures
{
    /// <summary>
    /// Account log
    /// </summary>
    public record KrakenAccountLogResult
    {
        /// <summary>
        /// Account id
        /// </summary>
        [JsonPropertyName("accountUid")]
        public string AccountUid { get; set; } = string.Empty;

        /// <summary>
        /// Log entries
        /// </summary>
        [JsonPropertyName("logs")]
        public IEnumerable<KrakenAccountLog> Logs { get; set; } = Array.Empty<KrakenAccountLog>();
    }

    /// <summary>
    /// Log entry
    /// </summary>
    public record KrakenAccountLog
    {
        /// <summary>
        /// The asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string? Asset { get; set; }
        /// <summary>
        /// Booking id
        /// </summary>
        [JsonPropertyName("booking_uid")]
        public string? BookingUid { get; set; } = string.Empty;
        /// <summary>
        /// Collateral
        /// </summary>
        [JsonPropertyName("collateral")]
        public string Collateral { get; set; } = string.Empty;
        /// <summary>
        /// Contract
        /// </summary>
        [JsonPropertyName("contract")]
        public string Contract { get; set; } = string.Empty;
        /// <summary>
        /// Conversion speed percentage
        /// </summary>
        [JsonPropertyName("conversion_spread_percentage")]
        public decimal? ConversionSpeedPercentage { get; set; }
        /// <summary>
        /// Event time
        /// </summary>
        [JsonPropertyName("date")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Execution
        /// </summary>
        [JsonPropertyName("execution")]
        public string Execution { get; set; } = string.Empty;
        /// <summary>
        /// Fee paid
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal? Fee { get; set; }
        /// <summary>
        /// Funding rate
        /// </summary>
        [JsonPropertyName("funding_rate")]
        public decimal? FundingRate { get; set; }
        /// <summary>
        /// Log id
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }
        /// <summary>
        /// Info
        /// </summary>
        [JsonPropertyName("info")]
        public string Info { get; set; } = string.Empty;
        /// <summary>
        /// Liquidation fee
        /// </summary>
        [JsonPropertyName("liquidation_fee")]
        public decimal? LiquidationFee { get; set; }
        /// <summary>
        /// Margin account
        /// </summary>
        [JsonPropertyName("margin_account")]
        public string? MarginAccount { get; set; }
        /// <summary>
        /// Mark price
        /// </summary>
        [JsonPropertyName("mark_price")]
        public decimal? MarkPrice { get; set; }
        /// <summary>
        /// New average entry price
        /// </summary>
        [JsonPropertyName("new_average_entry_price")]
        public decimal? NewAverageEntryPrice { get; set; }
        /// <summary>
        /// New balance
        /// </summary>
        [JsonPropertyName("new_balance")]
        public decimal? NewBalance { get; set; }
        /// <summary>
        /// Previous average entry price
        /// </summary>
        [JsonPropertyName("old_average_entry_price")]
        public decimal? OldAverageEntryPrice { get; set; }
        /// <summary>
        /// Previous balance
        /// </summary>
        [JsonPropertyName("old_balance")]
        public decimal? OldBalance { get; set; }
        /// <summary>
        /// Realized funding
        /// </summary>
        [JsonPropertyName("realized_funding")]
        public decimal? RealizedFunding { get; set; }
        /// <summary>
        /// Realized profit and loss
        /// </summary>
        [JsonPropertyName("realized_pnl")]
        public decimal? RealizedProfitAndLoss { get; set; }
        /// <summary>
        /// Trade price
        /// </summary>
        [JsonPropertyName("trade_price")]
        public decimal? TradePrice { get; set; }
    }
}
