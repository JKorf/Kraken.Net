using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Self trade strategy
    /// </summary>
    [JsonConverter(typeof(EnumConverter<SelfTradeStrategy>))]
    public enum SelfTradeStrategy
    {
        /// <summary>
        /// ["<c>REJECT_TAKER</c>"] Default behaviour, rejects the taker order that would match against a maker order from any sub-account
        /// </summary>
        [Map("REJECT_TAKER")]
        RejectTaker,
        /// <summary>
        /// ["<c>CANCEL_MAKER_SELF</c>"] Only cancels the maker order if it is from the same account that sent the taker order
        /// </summary>
        [Map("CANCEL_MAKER_SELF")]
        CancelMakerSelf,
        /// <summary>
        /// ["<c>CANCEL_MAKER_CHILD</c>"] Only allows master to cancel its own maker orders and orders from its sub-account
        /// </summary>
        [Map("CANCEL_MAKER_CHILD")]
        CancelMakerChild,
        /// <summary>
        /// ["<c>CANCEL_MAKER_ANY</c>"] Allows both master accounts and their subaccounts to cancel maker orders
        /// </summary>
        [Map("CANCEL_MAKER_ANY")]
        CancelMakerAny
    }
}
