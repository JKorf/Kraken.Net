using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Futures
{
    /// <summary>
    /// Account log
    /// </summary>
    public class KrakenAccountLogResult
    {
        /// <summary>
        /// Account id
        /// </summary>
        public string AccountUid { get; set; } = string.Empty;

        /// <summary>
        /// Log entries
        /// </summary>
        public IEnumerable<KrakenAccountLog> Logs { get; set; } = Array.Empty<KrakenAccountLog>();
    }

    /// <summary>
    /// Log entry
    /// </summary>
    public class KrakenAccountLog
    {
        /// <summary>
        /// The asset
        /// </summary>
        [JsonProperty("asset")]
        public string? Asset { get; set; }
        /// <summary>
        /// Booking id
        /// </summary>
        [JsonProperty("booking_uid")]
        public string? BookingUid { get; set; } = string.Empty;
        /// <summary>
        /// Collateral
        /// </summary>
        public string Collateral { get; set; } = string.Empty;
        /// <summary>
        /// Contract
        /// </summary>
        public string Contract { get; set; } = string.Empty;
        /// <summary>
        /// Conversion speed percentage
        /// </summary>
        [JsonProperty("conversion_spread_percentage")]
        public decimal? ConversionSpeedPercentage { get; set; }
        /// <summary>
        /// Event time
        /// </summary>
        [JsonProperty("date")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Execution
        /// </summary>
        public string Execution { get; set; } = string.Empty;
        /// <summary>
        /// Fee paid
        /// </summary>
        public decimal? Fee { get; set; }
        /// <summary>
        /// Funding rate
        /// </summary>
        [JsonProperty("funding_rate")]
        public decimal? FundingRate { get; set; }
        /// <summary>
        /// Log id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Info
        /// </summary>
        public string Info { get; set; } = string.Empty;
        /// <summary>
        /// Liquidation fee
        /// </summary>
        [JsonProperty("liquidation_fee")]
        public decimal? LiquidationFee { get; set; }
        /// <summary>
        /// Margin account
        /// </summary>
        [JsonProperty("margin_account")]
        public string? MarginAccount { get; set; }
        /// <summary>
        /// Mark price
        /// </summary>
        [JsonProperty("mark_price")]
        public decimal? MarkPrice { get; set; }
        /// <summary>
        /// New average entry price
        /// </summary>
        [JsonProperty("new_average_entry_price")]
        public decimal? NewAverageEntryPrice { get; set; }
        /// <summary>
        /// New balance
        /// </summary>
        [JsonProperty("new_balance")]
        public decimal? NewBalance { get; set; }
        /// <summary>
        /// Previous average entry price
        /// </summary>
        [JsonProperty("old_average_entry_price")]
        public decimal? OldAverageEntryPrice { get; set; }
        /// <summary>
        /// Previous balance
        /// </summary>
        [JsonProperty("old_balance")]
        public decimal? OldBalance { get; set; }
        /// <summary>
        /// Realized funding
        /// </summary>
        [JsonProperty("realized_funding")]
        public decimal? RealizedFunding { get; set; }
        /// <summary>
        /// Realized profit and loss
        /// </summary>
        [JsonProperty("realized_pnl")]
        public decimal? RealizedProfitAndLoss { get; set; }
        /// <summary>
        /// Trade price
        /// </summary>
        [JsonProperty("trade_price")]
        public decimal? TradePrice { get; set; }
    }
}
