using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Futures
{
    [SerializationModel]
    internal record KrakenFuturesTradeResult : KrakenFuturesResult<KrakenFuturesTrade[]>
    {
        [JsonPropertyName("history")]
        public override KrakenFuturesTrade[] Data { get; set; } = [];
    }

    /// <summary>
    /// Trade info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesTrade
    {
        /// <summary>
        /// ["<c>execution_venue</c>"] Execution venue
        /// </summary>
        [JsonPropertyName("execution_venue")]
        public string? ExecutionVenue { get; set; }

        /// <summary>
        /// ["<c>instrument_identification_type</c>"] Symbol identification type
        /// </summary>
        [JsonPropertyName("instrument_identification_type")]
        public string? SymbolIdentificationType { get; set; }

        /// <summary>
        /// ["<c>isin</c>"] Isin
        /// </summary>
        [JsonPropertyName("isin")]
        public string? IsIn { get; set; }

        /// <summary>
        /// ["<c>notional_amount</c>"] Notional amount
        /// </summary>
        [JsonPropertyName("notional_amount")]
        public decimal? NotionalAmount { get; set; }

        /// <summary>
        /// ["<c>notional_currency</c>"] Notional currency
        /// </summary>
        [JsonPropertyName("notional_currency")]
        public string? NotionalCurrency { get; set; }

        /// <summary>
        /// ["<c>price</c>"] For futures: The price of a fill. For indices: The calculated value
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        /// <summary>
        /// ["<c>price_currency</c>"] Price currency
        /// </summary>
        [JsonPropertyName("price_currency")]
        public string? PriceCurrency { get; set; }

        /// <summary>
        /// ["<c>price_notation</c>"] Price notification
        /// </summary>
        [JsonPropertyName("price_notation")]
        public string? PriceNotation { get; set; }

        /// <summary>
        /// ["<c>publication_time</c>"] Publication time
        /// </summary>
        [JsonPropertyName("publication_time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? PublicationTime { get; set; }

        /// <summary>
        /// ["<c>publication_venue</c>"] Publication venue
        /// </summary>
        [JsonPropertyName("publication_venue")]
        public string? PublicationVenue { get; set; }

        /// <summary>
        /// ["<c>side</c>"] The classification of the taker side in the matched trade: "buy" if the taker is a buyer, "sell" if the taker is a seller.
        /// </summary>

        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }

        /// <summary>
        /// ["<c>size</c>"] The size of a fill 
        /// </summary>
        [JsonPropertyName("size")]
        public decimal? Quantity { get; set; }

        /// <summary>
        /// ["<c>time</c>"] The date and time of a trade or an index computation. For futures: The date and time of a trade.Data is not aggregated For indices: The date and time of an index computation.For real-time indices, data is aggregated to the last computation of each full hour.For reference rates, data is not aggregated
        /// </summary>
        [JsonPropertyName("time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Timestamp { get; set; }

        /// <summary>
        /// ["<c>to_be_cleared</c>"] To be cleared
        /// </summary>
        [JsonPropertyName("to_be_cleared")]
        public bool? ToBeCleared { get; set; }

        /// <summary>
        /// ["<c>trade_id</c>"] A continuous index starting at 1 for the first fill in a Futures contract maturity
        /// </summary>
        [JsonPropertyName("trade_id")]
        public int? TradeId { get; set; }

        /// <summary>
        /// ["<c>transaction_identification_code</c>"] Transaction id code
        /// </summary>
        [JsonPropertyName("transaction_identification_code")]
        public string? TransactionIdentificationCode { get; set; }

        /// <summary>
        /// ["<c>type</c>"] Type
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// ["<c>uid</c>"] Uid
        /// </summary>
        [JsonPropertyName("uid")]
        public string? Uid { get; set; }
    }
}
