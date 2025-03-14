using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
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
        /// Maker
        /// </summary>
        [Map("maker", "m")]
        Maker,
        /// <summary>
        /// Taker
        /// </summary>
        [Map("taker", "t")]
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
