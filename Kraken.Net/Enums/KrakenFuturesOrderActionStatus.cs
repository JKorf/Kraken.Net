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
        /// ["<c>placed</c>"] The order was placed successfully 
        /// </summary>
        [Map("placed")]
        Placed,
        /// <summary>
        /// ["<c>partiallyFilled</c>"] Order was partially filled
        /// </summary>
        [Map("partiallyFilled")]
        PartiallyFilled,
        /// <summary>
        /// ["<c>filled</c>"] Order was filled
        /// </summary>
        [Map("filled")]
        Filled,
        /// <summary>
        /// ["<c>cancelled</c>"] The order was cancelled successfully
        /// </summary>
        [Map("cancelled")]
        Cancelled,
        /// <summary>
        /// ["<c>edited</c>"] Edited
        /// </summary>
        [Map("edited")]
        Edited,
        /// <summary>
        /// ["<c>marketSuspended</c>"] The order was not placed because the market is suspended
        /// </summary>
        [Map("marketSuspended")]
        MarketSuspended,
        /// <summary>
        /// ["<c>marketInactive</c>"] The order was not placed because the market is inactive
        /// </summary>
        [Map("marketInactive")]
        MarketInactive,
        /// <summary>
        /// ["<c>invalidPrice</c>"] The order was not placed because limitPrice and/or stopPrice are invalid
        /// </summary>
        [Map("invalidPrice")]
        InvalidPrice,
        /// <summary>
        /// ["<c>invalidSize</c>"] The order was not placed because size is invalid
        /// </summary>
        [Map("invalidSize")]
        InvalidSize,
        /// <summary>
        /// ["<c>tooManySmallOrders</c>"] The order was not placed because the number of small open orders would exceed the permissible limit
        /// </summary>
        [Map("tooManySmallOrders")]
        TooManySmallOrders,
        /// <summary>
        /// ["<c>insufficientAvailableFunds</c>"] The order was not placed because available funds are insufficient
        /// </summary>
        [Map("insufficientAvailableFunds")]
        InsufficientAvailableFunds,
        /// <summary>
        /// ["<c>wouldCauseLiquidation</c>"] Returned when a new order would fill at a worse price than the mark price, causing the portfolio value to fall below maintenance margin and triggering a liquidation
        /// </summary>
        [Map("wouldCauseLiquidation")]
        WouldCauseLiquidation,
        /// <summary>
        /// ["<c>clientOrderIdAlreadyExist</c>"] The specified client id already exist
        /// </summary>
        [Map("clientOrderIdAlreadyExist")]
        ClientOrderIdAlreadyExist,
        /// <summary>
        ///  ["<c>clientOrderIdTooBig</c>"] the client id is longer than the permissible limit
        /// </summary>
        [Map("clientOrderIdTooBig")]
        ClientOrderIdTooBig,
        /// <summary>
        /// ["<c>maxPositionViolation</c>"] Order would cause you to exceed your maximum position in this contract.
        /// </summary>
        [Map("maxPositionViolation")]
        MaxPositionViolation,
        /// <summary>
        /// ["<c>outsidePriceCollar</c>"] The order would have executed outside of the price collar for its order type
        /// </summary>
        [Map("outsidePriceCollar")]
        OutsidePriceCollar,
        /// <summary>
        /// ["<c>wouldIncreasePriceDislocation</c>"] Would increase price dislocation
        /// </summary>
        [Map("wouldIncreasePriceDislocation")]
        WouldIncreasePriceDislocation,
        /// <summary>
        /// ["<c>notFound</c>"] Not found
        /// </summary>
        [Map("notFound")]
        NotFound,
        /// <summary>
        /// ["<c>orderForEditNotAStop</c>"] Order not a stop order
        /// </summary>
        [Map("orderForEditNotAStop")]
        OrderForEditNotAStop,
        /// <summary>
        /// ["<c>orderForEditNotFound</c>"] Order not found
        /// </summary>
        [Map("orderForEditNotFound")]
        OrderForEditNotFound,
        /// <summary>
        /// ["<c>postWouldExecute</c>"] The post-only order would be filled upon placement, thus is cancelled
        /// </summary>
        [Map("postWouldExecute")]
        PostWouldExecute,
        /// <summary>
        /// ["<c>iocWouldNotExecute</c>"] the immediate-or-cancel order would not execute.
        /// </summary>
        [Map("iocWouldNotExecute")]
        IocWouldNotExecute,
        /// <summary>
        /// ["<c>selfFill</c>"] The order was not placed because it would be filled against an existing order belonging to the same account
        /// </summary>
        [Map("selfFill")]
        SelfFill,
        /// <summary>
        /// ["<c>wouldNotReducePosition</c>"] The reduce only order would not reduce position.
        /// </summary>
        [Map("wouldNotReducePosition")]
        WouldNotReducePosition,
        /// <summary>
        /// ["<c>marketIsPostOnly</c>"] The market is post only
        /// </summary>
        [Map("marketIsPostOnly")]
        MarketIsPostOnly,
        /// <summary>
        /// ["<c>tooManyOrders</c>"] Too many orders
        /// </summary>
        [Map("tooManyOrders")]
        TooManyOrders,
        /// <summary>
        /// ["<c>fixedLeverageTooHigh</c>"] Fixed leverage too high
        /// </summary>
        [Map("fixedLeverageTooHigh")]
        FixedLeverageTooHigh,
        /// <summary>
        /// ["<c>clientOrderIdInvalid</c>"] Client order id is invalid
        /// </summary>
        [Map("clientOrderIdInvalid")]
        ClientOrderIdInvalid,
        /// <summary>
        /// ["<c>cannotEditTriggerPriceOfTrailingStop</c>"] Cannot edit trigger price of trailing stop
        /// </summary>
        [Map("cannotEditTriggerPriceOfTrailingStop")]
        CannotEditTriggerPriceOfTrailingStop,
        /// <summary>
        /// ["<c>cannotEditLimitPriceOfTrailingStop</c>"] Cannot edit limit price of trailing stop
        /// </summary>
        [Map("cannotEditLimitPriceOfTrailingStop")]
        CannotEditLimitPriceOfTrailingStop
    }
}
