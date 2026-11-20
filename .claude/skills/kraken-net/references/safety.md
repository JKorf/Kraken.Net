# Kraken operational considerations

## Credentials, 2FA, and nonces

Spot and Futures use separate HMAC credential slots and can require different keys and permissions. Keep secrets in application secret storage. Private Spot WebSocket streams and order requests use Spot credentials and library-managed token acquisition; private Futures subscriptions use Futures credentials. Optional `twoFactorPassword` parameters apply to supported Spot private methods. `KrakenRestOptions.StaticTwoFactorAuthenticationPassword` configures a static value; it does not generate or refresh a time-based OTP.

When configuring a custom `NonceProvider`, ensure each later request for the key uses a higher nonce than those already sent. Sharing one API key across processes without coordinating nonce generation can invalidate requests. Do not reset the nonce to recover a transient transport error.

## Orders, amendments, and cancellation

Spot `validateOnly: true` validates an order without placing it. It is suitable for syntax examples that should not trade; it does not reserve funds or guarantee a later order will execute. Futures `PlaceOrderAsync` has no validation-only parameter. A limit order can execute immediately, and a cancellation can race with fills. Check order/trade state after submission and cancellation before deciding what exposure remains.

Spot WebSocket v2 exposes both `EditOrderAsync` (amend) and `ReplaceOrderAsync` (replacement); preserve their distinct parameters and returned identity rather than treating the names as interchangeable. Spot REST and socket batch placement return nested per-item `CallResult` values. Inspect every item after outer success; some orders may be accepted while others fail. Futures order-action results include status and order events; inspect returned identifiers/events rather than treating submission success as a fill.

Dead-man switch methods differ: Spot `CancelAllOrdersAfterAsync`, Futures `CancelAllOrderAfterAsync`. Keep any requested timeout refreshed for the intended lifetime and understand its cancellation scope; use the exposed disable semantics when intentionally removing protection.

## Futures positions and margin

`SetLeverageAsync` changes symbol leverage state and liquidation exposure. Read `FuturesApi.Trading.GetOpenPositionsAsync()` before deriving close quantity and side; use `reduceOnly` when the intent is to reduce exposure. Respect each contract's size/precision and linear/inverse settlement. Do not add another exchange's hedge-mode or position-side parameters to Kraken calls. Futures account transfers and PNL currency changes have account effects distinct from placing orders.

## Earn and funds movement

Earn allocation/deallocation moves funds into or out of a strategy; use its status endpoints to track the operation. Withdrawals use a configured withdrawal key/method and may take optional address and 2FA parameters. Match the key, asset, address, and amount to the requested destination. Wallet transfers require explicit source and destination wallets/accounts; Spot and Futures expose distinct transfer methods.

## Uncertain submissions and retries

`Error.IsTransient` alone does not make a signed account change safe to repeat. After timeout or connection loss, an order, withdrawal, transfer, allocation, or leverage change may already have been accepted. Retain client/order/reference identifiers and reconcile orders, trades, balances, ledger, allocation, or withdrawal status before deciding whether to resubmit. Account for status propagation delays and partial batch acceptance. Bounded backoff is appropriate for transient public reads; avoid using a generic read-retry helper for account changes.
