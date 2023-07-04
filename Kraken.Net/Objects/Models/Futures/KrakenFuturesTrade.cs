using CryptoExchange.Net.Converters;
using Kraken.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Futures
{
    internal class KrakenFuturesTradeResult : KrakenFuturesResult<IEnumerable<KrakenFuturesTrade>>
    {
        [JsonProperty("history")]
        public override IEnumerable<KrakenFuturesTrade> Data { get; set; } = new List<KrakenFuturesTrade>();
    }

    /// <summary>
    /// Trade info
    /// </summary>
    public class KrakenFuturesTrade
    {
        /// <summary>
        /// Execution venue
        /// </summary>
        [JsonProperty("execution_venue")]
        public string? ExecutionVenue { get; set; }

        /// <summary>
        /// Instrument identification type
        /// </summary>
        [JsonProperty("instrument_identification_type")]
        public string? InstrumentIdentificationType { get; set; }

        /// <summary>
        /// Isin
        /// </summary>
        public string? IsIn { get; set; }

        /// <summary>
        /// Notional amount
        /// </summary>
        [JsonProperty("notional_amount")]
        public decimal? NotionalAmount { get; set; }

        /// <summary>
        /// Notional currency
        /// </summary>
        [JsonProperty("notional_currency")]
        public string? NotionalCurrency { get; set; }

        /// <summary>
        /// For futures: The price of a fill. For indices: The calculated value
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Price currency
        /// </summary>
        [JsonProperty("price_currency")]
        public string? PriceCurrency { get; set; }

        /// <summary>
        /// Price notification
        /// </summary>
        [JsonProperty("price_notation")]
        public string? PriceNotation { get; set; }

        /// <summary>
        /// Publication time
        /// </summary>
        [JsonProperty("publication_time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? PublicationTime { get; set; }

        /// <summary>
        /// Publication venue
        /// </summary>
        [JsonProperty("publication_venue")]
        public string? PublicationVenue { get; set; }

        /// <summary>
        /// The classification of the taker side in the matched trade: "buy" if the taker is a buyer, "sell" if the taker is a seller.
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        public OrderSide Side { get; set; }

        /// <summary>
        /// The size of a fill 
        /// </summary>
        [JsonProperty("size")]
        public decimal? Quantity { get; set; }

        /// <summary>
        /// The date and time of a trade or an index computation. For futures: The date and time of a trade.Data is not aggregated For indices: The date and time of an index computation.For real-time indices, data is aggregated to the last computation of each full hour.For reference rates, data is not aggregated
        /// </summary>
        [JsonProperty("time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Timestamp { get; set; }

        /// <summary>
        /// To be cleared
        /// </summary>
        [JsonProperty("to_be_cleared")]
        public bool? ToBeCleared { get; set; }

        /// <summary>
        /// A continuous index starting at 1 for the first fill in a Futures contract maturity
        /// </summary>
        [JsonProperty("trade_id")]
        public int? TradeId { get; set; }

        /// <summary>
        /// Transaction id code
        /// </summary>
        [JsonProperty("transaction_identification_code")]
        public string? TransactionIdentificationCode { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Uid
        /// </summary>
        public string? Uid { get; set; }
    }
}
