using CryptoExchange.Net.Converters.SystemTextJson;
using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Futures
{
    [SerializationModel]
    internal record KrakenFuturesTradeResult : KrakenFuturesResult<KrakenFuturesTrade>
    {
        [JsonPropertyName("history")]
        public override KrakenFuturesTrade Data { get; set; } = new List<KrakenFuturesTrade>();
    }

    /// <summary>
    /// Trade info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesTrade
    {
        /// <summary>
        /// Execution venue
        /// </summary>
        [JsonPropertyName("execution_venue")]
        public string? ExecutionVenue { get; set; }

        /// <summary>
        /// Symbol identification type
        /// </summary>
        [JsonPropertyName("instrument_identification_type")]
        public string? SymbolIdentificationType { get; set; }

        /// <summary>
        /// Isin
        /// </summary>
        [JsonPropertyName("isin")]
        public string? IsIn { get; set; }

        /// <summary>
        /// Notional amount
        /// </summary>
        [JsonPropertyName("notional_amount")]
        public decimal? NotionalAmount { get; set; }

        /// <summary>
        /// Notional currency
        /// </summary>
        [JsonPropertyName("notional_currency")]
        public string? NotionalCurrency { get; set; }

        /// <summary>
        /// For futures: The price of a fill. For indices: The calculated value
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Price currency
        /// </summary>
        [JsonPropertyName("price_currency")]
        public string? PriceCurrency { get; set; }

        /// <summary>
        /// Price notification
        /// </summary>
        [JsonPropertyName("price_notation")]
        public string? PriceNotation { get; set; }

        /// <summary>
        /// Publication time
        /// </summary>
        [JsonPropertyName("publication_time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? PublicationTime { get; set; }

        /// <summary>
        /// Publication venue
        /// </summary>
        [JsonPropertyName("publication_venue")]
        public string? PublicationVenue { get; set; }

        /// <summary>
        /// The classification of the taker side in the matched trade: "buy" if the taker is a buyer, "sell" if the taker is a seller.
        /// </summary>

        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }

        /// <summary>
        /// The size of a fill 
        /// </summary>
        [JsonPropertyName("size")]
        public decimal? Quantity { get; set; }

        /// <summary>
        /// The date and time of a trade or an index computation. For futures: The date and time of a trade.Data is not aggregated For indices: The date and time of an index computation.For real-time indices, data is aggregated to the last computation of each full hour.For reference rates, data is not aggregated
        /// </summary>
        [JsonPropertyName("time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Timestamp { get; set; }

        /// <summary>
        /// To be cleared
        /// </summary>
        [JsonPropertyName("to_be_cleared")]
        public bool? ToBeCleared { get; set; }

        /// <summary>
        /// A continuous index starting at 1 for the first fill in a Futures contract maturity
        /// </summary>
        [JsonPropertyName("trade_id")]
        public int? TradeId { get; set; }

        /// <summary>
        /// Transaction id code
        /// </summary>
        [JsonPropertyName("transaction_identification_code")]
        public string? TransactionIdentificationCode { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Uid
        /// </summary>
        [JsonPropertyName("uid")]
        public string? Uid { get; set; }
    }
}
