namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Open positions update
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesOpenPositionUpdate : KrakenFuturesSocketMessage
    {
        /// <summary>
        /// ["<c>account</c>"] Account
        /// </summary>
        [JsonPropertyName("account")]
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>positions</c>"] Open positions
        /// </summary>
        [JsonPropertyName("positions")]
        public KrakenFuturesOpenPosition[] Positions { get; set; } = Array.Empty<KrakenFuturesOpenPosition>();
    }

    /// <summary>
    /// Open position info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesOpenPosition
    {
        /// <summary>
        /// ["<c>instrument</c>"] The symbol
        /// </summary>
        [JsonPropertyName("instrument")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>balance</c>"] The size of the position.
        /// </summary>
        [JsonPropertyName("balance")]
        public decimal Balance { get; set; }
        /// <summary>
        /// ["<c>entry_price</c>"] The average entry price of the symbol.
        /// </summary>
        [JsonPropertyName("entry_price")]
        public decimal EntryPrice { get; set; }
        /// <summary>
        /// ["<c>mark_price</c>"] The market price of the position symbol.
        /// </summary>
        [JsonPropertyName("mark_price")]
        public decimal MarkPrice { get; set; }
        /// <summary>
        /// ["<c>index_price</c>"] The index price of the position symbol.
        /// </summary>
        [JsonPropertyName("index_price")]
        public decimal IndexPrice { get; set; }
        /// <summary>
        /// ["<c>pnl</c>"] The profit and loss of the position.
        /// </summary>
        [JsonPropertyName("pnl")]
        public decimal ProfitAndLoss { get; set; }
        /// <summary>
        /// ["<c>liquidation_threshold</c>"] The mark price of the contract at which the position will be liquidated.
        /// </summary>
        [JsonPropertyName("liquidation_threshold")]
        public decimal LiquidationThreshold { get; set; }
        /// <summary>
        /// ["<c>return_on_equity</c>"] The percentage gain or loss relative to the initial margin used in the position. Formula: PnL/IM
        /// </summary>
        [JsonPropertyName("return_on_equity")]
        public decimal ReturnOnEquity { get; set; }
        /// <summary>
        /// ["<c>effective_leverage</c>"] How leveraged the net position is in a given margin account. Formula: Position Value at Market / Portfolio Value.
        /// </summary>
        [JsonPropertyName("effective_leverage")]
        public decimal EffectiveLeverage { get; set; }
    }
}
