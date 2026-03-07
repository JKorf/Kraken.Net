using Kraken.Net.Enums;
using Kraken.Net.Objects.Models;

namespace Kraken.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Kraken trading endpoints, placing and managing orders.
    /// </summary>
    public interface IKrakenRestClientSpotApiTrading
    {
        /// <summary>
        /// Get a list of open orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/get-open-orders" /><br />
        /// Endpoint:<br />
        /// POST /0/private/OpenOrders
        /// </para>
        /// </summary>
        /// <param name="userReference">["<c>userref</c>"] Filter by user reference</param>
        /// <param name="clientOrderId">Filter by client order id</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of open orders</returns>
        Task<WebCallResult<OpenOrdersPage>> GetOpenOrdersAsync(uint? userReference = null, string? clientOrderId = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get a list of closed orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/get-closed-orders" /><br />
        /// Endpoint:<br />
        /// POST /0/private/ClosedOrders
        /// </para>
        /// </summary>
        /// <param name="userRef">["<c>userref</c>"] Filter by user reference</param>
        /// <param name="startTime">["<c>start</c>"] Return data after this time</param>
        /// <param name="endTime">["<c>end</c>"] Return data before this time</param>
        /// <param name="resultOffset">["<c>ofs</c>"] Offset the results by</param>
        /// <param name="clientOrderId">["<c>cl_ord_id</c>"] Filter by client order id</param>
        /// <param name="searchTime">["<c>closetime</c>"] Which time to use when searching</param>
        /// <param name="consolidateTaker">["<c>consolidate_taker</c>"] Whether or not to consolidate trades by individual taker trades</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Closed orders page</returns>
        Task<WebCallResult<KrakenClosedOrdersPage>> GetClosedOrdersAsync(uint? userRef = null, DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null, string? clientOrderId = null, SearchTime? searchTime = null, bool? consolidateTaker = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get info on specific order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/get-orders-info" /><br />
        /// Endpoint:<br />
        /// POST /0/private/QueryOrders
        /// </para>
        /// </summary>
        /// <param name="clientOrderId">["<c>userref</c>"] Get orders by clientOrderId</param>
        /// <param name="orderId">["<c>txid</c>"] Get order by its order id</param>
        /// <param name="consolidateTaker">["<c>consolidate_taker</c>"] Whether or not to consolidate trades by individual taker trades</param>
        /// <param name="trades">["<c>trades</c>"] Whether to include trades in the response</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with order info</returns>
        Task<WebCallResult<Dictionary<string, KrakenOrder>>> GetOrderAsync(string? orderId = null, uint? clientOrderId = null, bool? consolidateTaker = null, bool? trades = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get info on specific orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/get-orders-info" /><br />
        /// Endpoint:<br />
        /// POST /0/private/QueryOrders
        /// </para>
        /// </summary>
        /// <param name="clientOrderId">["<c>userref</c>"] Get orders by clientOrderId</param>
        /// <param name="orderIds">["<c>txid</c>"] Get orders by their order ids</param>
        /// <param name="consolidateTaker">["<c>consolidate_taker</c>"] Whether or not to consolidate trades by individual taker trades</param>
        /// <param name="trades">["<c>trades</c>"] Whether to include trades in the response</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with order info</returns>
        Task<WebCallResult<Dictionary<string, KrakenOrder>>> GetOrdersAsync(IEnumerable<string>? orderIds = null, uint? clientOrderId = null, bool? consolidateTaker = null, bool? trades = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get trade history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/get-trade-history" /><br />
        /// Endpoint:<br />
        /// POST /0/private/TradesHistory
        /// </para>
        /// </summary>
        /// <param name="startTime">["<c>start</c>"] Return data after this time</param>
        /// <param name="endTime">["<c>end</c>"] Return data before this time</param>
        /// <param name="resultOffset">["<c>ofs</c>"] Offset the results by</param>
        /// <param name="consolidateTaker">["<c>consolidate_taker</c>"] Whether or not to consolidate trades by individual taker trades</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Trade history page</returns>
        Task<WebCallResult<KrakenUserTradesPage>> GetUserTradesAsync(DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null, bool? consolidateTaker = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get info on specific trades
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/get-trades-info" /><br />
        /// Endpoint:<br />
        /// POST /0/private/QueryTrades
        /// </para>
        /// </summary>
        /// <param name="tradeId">["<c>txid</c>"] The trade to get info on</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with trade info</returns>
        Task<WebCallResult<Dictionary<string, KrakenUserTrade>>> GetUserTradeDetailsAsync(string tradeId, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get info on specific trades
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/get-trades-info" /><br />
        /// Endpoint:<br />
        /// POST /0/private/QueryTrades
        /// </para>
        /// </summary>
        /// <param name="tradeIds">["<c>txid</c>"] The trades to get info on</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with trade info</returns>
        Task<WebCallResult<Dictionary<string, KrakenUserTrade>>> GetUserTradeDetailsAsync(IEnumerable<string> tradeIds, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Place multiple new orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/add-order-batch" /><br />
        /// Endpoint:<br />
        /// POST /0/private/AddOrderBatch
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>pair</c>"] The symbol the order is on, for example `ETHUSDT`</param>
        /// <param name="orders">["<c>orders</c>"] The orders to place</param>
        /// <param name="deadline">["<c>deadline</c>"] Deadline after which the orders will be rejected</param>
        /// <param name="validateOnly">["<c>validate</c>"] Only validate inputs, don't actually place the order</param>
        /// <param name="aClass">["<c>asset_class</c>"] Asset class, required to be `TokenizedAsset` when targeting xstocks</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<CallResult<KrakenPlacedBatchOrder>[]>> PlaceMultipleOrdersAsync(
            string symbol, 
            IEnumerable<KrakenOrderRequest> orders,
            DateTime? deadline = null,
            bool? validateOnly = null,
            AClass? aClass = null,
            CancellationToken ct = default);

        /// <summary>
        /// Place a new order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/add-order" /><br />
        /// Endpoint:<br />
        /// POST /0/private/AddOrder
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>pair</c>"] The symbol the order is on, for example `ETHUSDT`</param>
        /// <param name="side">["<c>type</c>"] The side of the order</param>
        /// <param name="type">["<c>ordertype</c>"] The type of the order</param>
        /// <param name="quantity">["<c>volume</c>"] The quantity of the order</param>
        /// <param name="userReference">["<c>userref</c>"] A numeric id to reference the order by</param>
        /// <param name="clientOrderId">["<c>cl_ord_id</c>"] A client id to reference the order by</param>
        /// <param name="price">["<c>price</c>"] Price of the order:<br />
        /// Limit=limit price<br />
        /// StopLoss=stop loss price<br />
        /// TakeProfit=take profit price<br />
        /// StopLossProfit=stop loss price<br />
        /// StopLossProfitLimit=stop loss price<br />
        /// StopLossLimit=stop loss trigger price<br />
        /// TakeProfitLimit=take profit trigger price<br />
        /// TrailingStop=trailing stop offset<br />
        /// TrailingStopLimit=trailing stop offset<br />
        /// StopLossAndLimit=stop loss price
        /// </param>
        /// <param name="secondaryPrice">["<c>price2</c>"] Secondary price of an order:
        /// StopLossProfit/StopLossProfitLimit=take profit price<br />
        /// StopLossLimit/TakeProfitLimit=triggered limit price<br />
        /// TrailingStopLimit=triggered limit offset<br />
        /// StopLossAndLimit=limit price</param>
        /// <param name="leverage">["<c>leverage</c>"] Desired leverage</param>
        /// <param name="startTime">["<c>starttm</c>"] Scheduled start time</param>
        /// <param name="expireTime">["<c>expiretm</c>"] Expiration time</param>
        /// <param name="validateOnly">["<c>validate</c>"] Only validate inputs, don't actually place the order</param>
        /// <param name="orderFlags">["<c>oflags</c>"] Flags for the order</param>
        /// <param name="reduceOnly">["<c>reduce_only</c>"] Reduce only order</param>
        /// <param name="icebergQuantity">["<c>displayvol</c>"] Iceberg visible quantity</param>
        /// <param name="trigger">["<c>trigger</c>"] Price signal</param>
        /// <param name="selfTradePreventionType">["<c>stptype</c>"] Self trade prevention type</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="timeInForce">["<c>timeinforce</c>"] Time-in-force of the order to specify how long it should remain in the order book before being cancelled</param>
        /// <param name="closeOrderType">["<c>close.ordertype</c>"] Close order type</param>
        /// <param name="closePrice">["<c>close.price</c>"] Close order price</param>
        /// <param name="secondaryClosePrice">["<c>close.price2</c>"] Close order secondary price</param>
        /// <param name="pricePrefixOperator">Prefix operator for the price field, `+` or `-` for a relative offset to the last traded price, `#` will either add or subtract the amount to the last traded price, depending on the direction and order type used. For trailing stop orders always `+` can be used for an absolute price</param>
        /// <param name="priceSuffixOperator">Suffix operator for the price field. Only option is `%` to specify a relative percentage offset for trailing orders</param>
        /// <param name="secondaryPricePrefixOperator">Prefix operator for the secondary price field, `+` or `-` for a relative offset to the last traded price, `#` will either add or subtract the amount to the last traded price, depending on the direction and order type used.</param>
        /// <param name="secondaryPriceSuffixOperator">Suffix operator for the price field. Only option is `%` to specify a relative percentage offset for trailing orders</param>
        /// <param name="aClass">["<c>asset_class</c>"] Asset class, required to be `TokenizedAsset` when targeting xstocks</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Placed order info</returns>
        Task<WebCallResult<KrakenPlacedOrder>> PlaceOrderAsync(
            string symbol,
            OrderSide side,
            OrderType type,
            decimal quantity,
            decimal? price = null,
            decimal? secondaryPrice = null,
            decimal? leverage = null,
            DateTime? startTime = null,
            DateTime? expireTime = null,
            bool? validateOnly = null,
            uint? userReference = null,
            string? clientOrderId = null,
            IEnumerable<OrderFlags>? orderFlags = null,
            string? twoFactorPassword = null,
            TimeInForce? timeInForce = null,
            bool? reduceOnly = null,
            decimal? icebergQuantity = null,
            Trigger? trigger = null,
            SelfTradePreventionType? selfTradePreventionType = null,
            OrderType? closeOrderType = null,
            decimal? closePrice = null,
            decimal? secondaryClosePrice = null,
            string? pricePrefixOperator = null,
            string? priceSuffixOperator = null,
            string? secondaryPricePrefixOperator = null,
            string? secondaryPriceSuffixOperator = null,
            AClass? aClass = null,
            CancellationToken ct = default);

        /// <summary>
        /// Edit an order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/edit-order" /><br />
        /// Endpoint:<br />
        /// POST /0/private/EditOrder
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>pair</c>"] Symbol, for example `ETHUSDT`</param>
        /// <param name="orderId">["<c>txid</c>"] Order id or client order id of the order to edit</param>
        /// <param name="quantity">["<c>volume</c>"] New quantity</param>
        /// <param name="icebergQuantity">["<c>displayvol</c>"] Iceberg quantity</param>
        /// <param name="price">["<c>price</c>"] Price</param>
        /// <param name="secondaryPrice">["<c>price2</c>"] Secondary price</param>
        /// <param name="flags">["<c>oflags</c>"] Flags</param>
        /// <param name="deadline">["<c>deadline</c>"] Deadline</param>
        /// <param name="cancelResponse">["<c>cancel_response</c>"] Used to interpret if client wants to receive pending replace, before the order is completely replaced</param>
        /// <param name="validateOnly">["<c>validate</c>"] Only validate inputs, don't actually place the order</param>
        /// <param name="newClientOrderId">["<c>userref</c>"] New client order id</param>
        /// <param name="pricePrefixOperator">Prefix operator for the price field, `+` or `-` for a relative offset to the last traded price, `#` will either add or subtract the amount to the last traded price, depending on the direction and order type used. For trailing stop orders always `+` can be used for an absolute price</param>
        /// <param name="priceSuffixOperator">Suffix operator for the price field. Only option is `%` to specify a relative percentage offset for trailing orders</param>
        /// <param name="secondaryPricePrefixOperator">Prefix operator for the secondary price field, `+` or `-` for a relative offset to the last traded price, `#` will either add or subtract the amount to the last traded price, depending on the direction and order type used.</param>
        /// <param name="secondaryPriceSuffixOperator">Suffix operator for the price field. Only option is `%` to specify a relative percentage offset for trailing orders</param>
        /// <param name="assetClass">["<c>asset_class</c>"] Asset class, required to be `TokenizedAsset` when targeting xstocks</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenEditOrder>> EditOrderAsync(
            string symbol,
            string orderId,
            decimal? quantity = null,
            decimal? icebergQuantity = null,
            decimal? price = null,
            decimal? secondaryPrice = null,
            IEnumerable<OrderFlags>? flags = null,
            DateTime? deadline = null,
            bool? cancelResponse = null,
            bool? validateOnly = null,
            uint? newClientOrderId = null,
            string? twoFactorPassword = null,
            string? pricePrefixOperator = null,
            string? priceSuffixOperator = null,
            string? secondaryPricePrefixOperator = null,
            string? secondaryPriceSuffixOperator = null,
            AClass? assetClass = null,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel an order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/cancel-order" /><br />
        /// Endpoint:<br />
        /// POST /0/private/CancelOrder
        /// </para>
        /// </summary>
        /// <param name="orderId">["<c>txid</c>"] The id of the order to cancel. Either this or clientOrderId should be provided</param>
        /// <param name="clientOrderId">["<c>cl_ord_id</c>"] The client order id of the order to cancel. Either this or orderId should be provided</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Cancel result</returns>
        Task<WebCallResult<KrakenCancelResult>> CancelOrderAsync(string? orderId = null, string? clientOrderId = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Cancel all orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/cancel-all-orders" /><br />
        /// Endpoint:<br />
        /// POST /0/private/CancelAll
        /// </para>
        /// </summary>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Cancel result</returns>
        Task<WebCallResult<KrakenCancelResult>> CancelAllOrdersAsync(string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Cancel all orders after the timeout expires. Can be called at an interval to keep extending the timeout and word as a dead-man switch. Timeout can be disabled by setting timeout to 0.
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/cancel-all-orders-after" /><br />
        /// Endpoint:<br />
        /// POST /0/private/CancelAllOrdersAfter
        /// </para>
        /// </summary>
        /// <param name="cancelAfter">["<c>timeout</c>"] Cancel after time</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenCancelAfterResult>> CancelAllOrdersAfterAsync(TimeSpan cancelAfter, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Cancel multiple orders by id or clientOrderId
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/cancel-order-batch/" /><br />
        /// Endpoint:<br />
        /// POST /0/private/CancelOrderBatch
        /// </para>
        /// </summary>
        /// <param name="orderIds">["<c>orders</c>"] Cancel by order ids</param>
        /// <param name="clientOrderIds">["<c>cl_ord_ids</c>"] Cancel by client order ids</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenBatchCancelResult>> CancelMultipleOrdersAsync(IEnumerable<string>? orderIds = null, IEnumerable<string>? clientOrderIds = null, string? twoFactorPassword = null, CancellationToken ct = default);
    }
}
