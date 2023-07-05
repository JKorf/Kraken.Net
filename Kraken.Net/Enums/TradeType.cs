using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Trade type
    /// </summary>
    public enum TradeType
    {
        /// <summary>
        /// Maker
        /// </summary>
        [Map("maker")]
        Maker,
        /// <summary>
        /// Taker
        /// </summary>
        [Map("taker")]
        Taker,
        /// <summary>
        /// Liquidation
        /// </summary>
        [Map("liquidation")]
        Liquidation,
        /// <summary>
        /// Assignee
        /// </summary>
        [Map("assignee")]
        Assignee,
        /// <summary>
        /// Assignor
        /// </summary>
        [Map("assignor")]
        Assignor,
        /// <summary>
        /// Taker after edit
        /// </summary>
        [Map("takerAfterEdit")]
        TakerAfterEdit,
        /// <summary>
        /// Unwinding bankrupt
        /// </summary>
        [Map("unwindBankrupt")]
        UnwindBankrupt,
        /// <summary>
        /// Unwinding country party
        /// </summary>
        [Map("unwindCounterparty")]
        UnwindCounterParty
    }
}
