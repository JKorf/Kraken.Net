using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Order action result
    /// </summary>
    [JsonConverter(typeof(EnumConverter<KrakenFuturesOrderActionStatus>))]
    public enum KrakenFuturesOrderActionStatus
    {
        /// <summary>
        /// The order was placed successfully 
        /// </summary>
        [Map("placed")]
        Placed,
        /// <summary>
        /// Order was partially filled
        /// </summary>
        [Map("partiallyFilled")]
        PartiallyFilled,
        /// <summary>
        /// Order was filled
        /// </summary>
        [Map("filled")]
        Filled,
        /// <summary>
        /// The order was cancelled successfully
        /// </summary>
        [Map("cancelled")]
        Cancelled,
        /// <summary>
        /// Edited
        /// </summary>
        [Map("edited")]
        Edited,
        /// <summary>
        /// The order was not placed because the market is suspended
        /// </summary>
        [Map("marketSuspended")]
        MarketSuspended,
        /// <summary>
        /// The order was not placed because the market is inactive
        /// </summary>
        [Map("marketInactive")]
        MarketInactive,
        /// <summary>
        /// The order was not placed because limitPrice and/or stopPrice are invalid
        /// </summary>
        [Map("InvalidPrice")]
        InvalidPrice,
        /// <summary>
        /// The order was not placed because size is invalid
        /// </summary>
        [Map("invalidSize")]
        InvalidSize,
        /// <summary>
        /// The order was not placed because the number of small open orders would exceed the permissible limit
        /// </summary>
        [Map("tooManySmallOrders")]
        TooManySmallOrders,
        /// <summary>
        /// The order was not placed because available funds are insufficient
        /// </summary>
        [Map("insufficientAvailableFunds")]
        InsufficientAvailableFunds,
        /// <summary>
        /// Returned when a new order would fill at a worse price than the mark price, causing the portfolio value to fall below maintenance margin and triggering a liquidation
        /// </summary>
        [Map("wouldCauseLiquidation")]
        WouldCauseLiquidation,
        /// <summary>
        /// The specified client id already exist
        /// </summary>
        [Map("clientOrderIdAlreadyExist")]
        ClientOrderIdAlreadyExist,
        /// <summary>
        ///  the client id is longer than the permissible limit
        /// </summary>
        [Map("clientOrderIdTooBig")]
        ClientOrderIdTooBig,
        /// <summary>
        /// Order would cause you to exceed your maximum position in this contract.
        /// </summary>
        [Map("maxPositionViolation")]
        MaxPositionViolation,
        /// <summary>
        /// The order would have executed outside of the price collar for its order type
        /// </summary>
        [Map("outsidePriceCollar")]
        OutsidePriceCollar,
        /// <summary>
        /// Would increase price dislocation
        /// </summary>
        [Map("wouldIncreasePriceDislocation")]
        WouldIncreasePriceDislocation,
        /// <summary>
        /// Not found
        /// </summary>
        [Map("notFound")]
        NotFound,
        /// <summary>
        /// Order not a stop order
        /// </summary>
        [Map("orderForEditNotAStop")]
        OrderForEditNotAStop,
        /// <summary>
        /// Order not found
        /// </summary>
        [Map("orderForEditNotFound")]
        OrderForEditNotFound,
        /// <summary>
        /// The post-only order would be filled upon placement, thus is cancelled
        /// </summary>
        [Map("postWouldExecute")]
        PostWouldExecute,
        /// <summary>
        /// the immediate-or-cancel order would not execute.
        /// </summary>
        [Map("iocWouldNotExecute")]
        IocWouldNotExecute,
        /// <summary>
        /// The order was not placed because it would be filled against an existing order belonging to the same account
        /// </summary>
        [Map("selfFill")]
        SelfFill,
        /// <summary>
        /// The reduce only order would not reduce position.
        /// </summary>
        [Map("wouldNotReducePosition")]
        WouldNotReducePosition,
        /// <summary>
        /// The market is post only
        /// </summary>
        [Map("marketIsPostOnly")]
        MarketIsPostOnly,
        /// <summary>
        /// Too many orders
        /// </summary>
        [Map("tooManyOrders")]
        TooManyOrders,
        /// <summary>
        /// Fixed leverage too high
        /// </summary>
        [Map("fixedLeverageTooHigh")]
        FixedLeverageTooHigh,
        /// <summary>
        /// Client order id is invalid
        /// </summary>
        [Map("clientOrderIdInvalid")]
        ClientOrderIdInvalid,
        /// <summary>
        /// Cannot edit trigger price of trailing stop
        /// </summary>
        [Map("cannotEditTriggerPriceOfTrailingStop")]
        CannotEditTriggerPriceOfTrailingStop,
        /// <summary>
        /// Cannot edit limit price of trailling stop
        /// </summary>
        [Map("cannotEditLimitPriceOfTrailingStop")]
        CannotEditLimitPriceOfTrailingStop
    }
}
