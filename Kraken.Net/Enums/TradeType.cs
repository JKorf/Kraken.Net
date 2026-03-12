using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Trade type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TradeType>))]
    public enum TradeType
    {
        /// <summary>
        /// ["<c>maker</c>"] Maker
        /// </summary>
        [Map("maker", "m")]
        Maker,
        /// <summary>
        /// ["<c>taker</c>"] Taker
        /// </summary>
        [Map("taker", "t")]
        Taker,
        /// <summary>
        /// ["<c>liquidation</c>"] Liquidation
        /// </summary>
        [Map("liquidation")]
        Liquidation,
        /// <summary>
        /// ["<c>assignee</c>"] Assignee
        /// </summary>
        [Map("assignee")]
        Assignee,
        /// <summary>
        /// ["<c>assignor</c>"] Assignor
        /// </summary>
        [Map("assignor")]
        Assignor,
        /// <summary>
        /// ["<c>takerAfterEdit</c>"] Taker after edit
        /// </summary>
        [Map("takerAfterEdit")]
        TakerAfterEdit,
        /// <summary>
        /// ["<c>unwindBankrupt</c>"] Unwinding bankrupt
        /// </summary>
        [Map("unwindBankrupt")]
        UnwindBankrupt,
        /// <summary>
        /// ["<c>unwindCounterparty</c>"] Unwinding country party
        /// </summary>
        [Map("unwindCounterparty")]
        UnwindCounterParty
    }
}
