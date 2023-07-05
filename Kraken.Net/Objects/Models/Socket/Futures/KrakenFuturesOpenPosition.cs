using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Open positions update
    /// </summary>
    public class KrakenFuturesOpenPositionUpdate : KrakenFuturesSocketMessage
    {
        /// <summary>
        /// Account
        /// </summary>
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// Open positions
        /// </summary>
        public IEnumerable<KrakenFuturesOpenPosition> Positions { get; set; } = Array.Empty<KrakenFuturesOpenPosition>();
    }

    /// <summary>
    /// Open position info
    /// </summary>
    public class KrakenFuturesOpenPosition
    {
        /// <summary>
        /// The symbol
        /// </summary>
        [JsonProperty("instrument")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// The size of the position.
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// The average entry price of the symbol.
        /// </summary>
        [JsonProperty("entry_price")]
        public decimal EntryPrice { get; set; }
        /// <summary>
        /// The market price of the position symbol.
        /// </summary>
        [JsonProperty("mark_price")]
        public decimal MarkPrice { get; set; }
        /// <summary>
        /// The index price of the position symbol.
        /// </summary>
        [JsonProperty("index_price")]
        public decimal IndexPrice { get; set; }
        /// <summary>
        /// The profit and loss of the position.
        /// </summary>
        [JsonProperty("pnl")]
        public decimal ProfitAndLoss { get; set; }
        /// <summary>
        /// The mark price of the contract at which the position will be liquidated.
        /// </summary>
        [JsonProperty("liquidation_threshold")]
        public decimal LiquidationThreshold { get; set; }
        /// <summary>
        /// The percentage gain or loss relative to the initial margin used in the position. Formula: PnL/IM
        /// </summary>
        [JsonProperty("return_on_equity")]
        public decimal ReturnOnEquity { get; set; }
        /// <summary>
        /// How leveraged the net position is in a given margin account. Formula: Position Value at Market / Portfolio Value.
        /// </summary>
        [JsonProperty("effective_leverage")]
        public decimal EffectiveLeverage { get; set; }
    }
}
