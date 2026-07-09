using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Order update reason
    /// </summary>
    [JsonConverter(typeof(EnumConverter<KrakenFuturesOrderUpdateReason>))]
    public enum KrakenFuturesOrderUpdateReason
    {
        /// <summary>
        /// User placed a new order
        /// </summary>
        [Map("new_placed_order_by_user")]
        NewPlacedOrderByUser,
        /// <summary>
        /// User position liquidated. the order cancelled
        /// </summary>
        [Map("liquidation")]
        Liquidation,
        /// <summary>
        /// A stop order triggered. the system removed the stop order
        /// </summary>
        [Map("stop_order_triggered")]
        StopOrderTriggered,
        /// <summary>
        /// The system created a limit order because an existing stop order triggered
        /// </summary>
        [Map("limit_order_from_stop")]
        LimitOrderFromStop,
        /// <summary>
        /// The order filled partially
        /// </summary>
        [Map("partial_fill")]
        PartialFill,
        /// <summary>
        /// The order filled fully and removed
        /// </summary>
        [Map("full_fill")]
        FullFill,
        /// <summary>
        /// The order cancelled by the user and removed
        /// </summary>
        [Map("cancelled_by_user")]
        CancelledByUser,
        /// <summary>
        /// The order contract expired. all open orders of that contract removed
        /// </summary>
        [Map("contract_expired")]
        ContractExpired,
        /// <summary>
        /// The order removed due to insufficient margin
        /// </summary>
        [Map("not_enough_margin")]
        NotEnoughMargin,
        /// <summary>
        /// The order removed because market became inactive
        /// </summary>
        [Map("market_inactive")]
        MarketInactive,
        /// <summary>
        /// The order removed by administrator’s action
        /// </summary>
        [Map("cancelled_by_admin")]
        CancelledByAdmin,
        /// <summary>
        /// The order removed because dead man’s switch was triggered
        /// </summary>
        [Map("dead_man_switch")]
        DeadManSwitch,
        /// <summary>
        /// The immediate or cancel order was rejected due to insufficient liquidity
        /// </summary>
        [Map("ioc_order_failed_because_it_would_not_be_executed")]
        IocOrderFailedBecauseItWouldNotBeExecuted,
        /// <summary>
        /// The post only order was rejected as it crosses the spread and would be immediately filled
        /// </summary>
        [Map("post_order_failed_because_it_would_filled")]
        PostOrderFailedBecauseItWouldFilled,
        /// <summary>
        /// The order was rejected as it would execute against another order from the same account
        /// </summary>
        [Map("would_execute_self")]
        WouldExecuteSelf,
        /// <summary>
        /// The order was rejected as the reduce-only option was selected and it would not reduce the position
        /// </summary>
        [Map("would_not_reduce_position")]
        WouldNotReducePosition,
        /// <summary>
        /// The order edit was rejected as the order to be edited could not be found
        /// </summary>
        [Map("order_for_edit_not_found")]
        OrderForEditNotFound,
    }

}
