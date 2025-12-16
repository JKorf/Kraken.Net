using CryptoExchange.Net.Objects.Errors;

namespace Kraken.Net
{
    internal static class KrakenErrors
    {
        public static ErrorMapping SpotMapping = new ErrorMapping([],
            [
                new ErrorEvaluator("EGeneral", (code, msg) => {
                    if(string.IsNullOrEmpty(msg))
                        return new ErrorInfo(ErrorType.Unknown, false, "Unknown error", code);

                    if (msg!.Equals("Invalid arguments"))
                        return new ErrorInfo(ErrorType.InvalidParameter, false, "Invalid parameters", code);

                    if (msg!.Equals("Temporary lockout"))
                        return new ErrorInfo(ErrorType.RateLimitRequest, false, "Too many failed requests", code);

                    if (msg!.Equals("Permission denied"))
                        return new ErrorInfo(ErrorType.Unauthorized, false, "Insufficient permissions", code);

                    if (msg!.StartsWith("Internal error"))
                        return new ErrorInfo(ErrorType.SystemError, false, "Internal system error", code);

                    return new ErrorInfo(ErrorType.Unknown, false, msg!, code);
                }),
            new ErrorEvaluator("EAccount", (code, msg) => {
                    if(string.IsNullOrEmpty(msg))
                        return new ErrorInfo(ErrorType.Unknown, false, "Unknown error", code);

                    if (msg!.Equals("Invalid permissions"))
                        return new ErrorInfo(ErrorType.Unauthorized, false, "Insufficient permissions", code);

                    return new ErrorInfo(ErrorType.Unknown, false, msg!, code);
                }),
            new ErrorEvaluator("EAuth", (code, msg) => {
                    if(string.IsNullOrEmpty(msg))
                        return new ErrorInfo(ErrorType.Unknown, false, "Unknown error", code);

                    if (msg!.Equals("Account temporary disabled"))
                        return new ErrorInfo(ErrorType.Unauthorized, false, "Account temporarily disabled", code);

                    if (msg!.Equals("Account unconfirmed"))
                        return new ErrorInfo(ErrorType.Unauthorized, false, "Account unconfirmed", code);

                    if (msg!.Equals("Rate limit exceeded") || msg.Equals("Too many requests"))
                        return new ErrorInfo(ErrorType.RateLimitRequest, false, "Too many requests", code);

                    return new ErrorInfo(ErrorType.Unknown, false, msg!, code);
                }),
            new ErrorEvaluator("EAPI", (code, msg) => {
                    if(string.IsNullOrEmpty(msg))
                        return new ErrorInfo(ErrorType.Unknown, false, "Unknown error", code);

                    if (msg!.Equals("Invalid key"))
                        return new ErrorInfo(ErrorType.Unauthorized, false, "Invalid API key", code);

                    if (msg!.Equals("Invalid signature"))
                        return new ErrorInfo(ErrorType.Unauthorized, false, "Invalid signature", code);

                    if (msg!.Equals("Invalid nonce"))
                        return new ErrorInfo(ErrorType.InvalidTimestamp, false, "Invalid nonce", code);

                    return new ErrorInfo(ErrorType.Unknown, false, msg!, code);
                }),
            new ErrorEvaluator("EQuery", (code, msg) => {
                    if(string.IsNullOrEmpty(msg))
                        return new ErrorInfo(ErrorType.Unknown, false, "Unknown error", code);

                    if (msg!.Equals("Unknown asset pair"))
                        return new ErrorInfo(ErrorType.UnknownSymbol, false, "Unknown symbol", code);

                    return new ErrorInfo(ErrorType.Unknown, false, msg!, code);
                }),
            new ErrorEvaluator("EOrder", (code, msg) => {
                    if(string.IsNullOrEmpty(msg))
                        return new ErrorInfo(ErrorType.Unknown, false, "Unknown error", code);

                    if (msg!.Equals("Cannot open opposing position") || msg.Equals("Cannot open position"))
                        return new ErrorInfo(ErrorType.Unauthorized, false, "User/tier is ineligible for margin trading", code);

                    if (msg!.Equals("Margin level too low"))
                        return new ErrorInfo(ErrorType.InsufficientBalance, false, "Insufficient margin available", code);

                    if (msg!.Equals("Insufficient funds"))
                        return new ErrorInfo(ErrorType.InsufficientBalance, false, "Insufficient balance", code);

                    if (msg!.Equals("Order minimum not met"))
                        return new ErrorInfo(ErrorType.InvalidQuantity, false, "Order quantity too low", code);

                    if (msg!.Equals("Cost minimum not met"))
                        return new ErrorInfo(ErrorType.InvalidQuantity, false, "Order value too low", code);

                    if (msg!.Equals("Tick size check failed"))
                        return new ErrorInfo(ErrorType.InvalidPrice, false, "Order price not a multiple of price tick increment", code);

                    if (msg!.Equals("Invalid price"))
                        return new ErrorInfo(ErrorType.InvalidPrice, false, "Order price invalid", code);

                    if (msg!.Equals("Orders limit exceeded"))
                        return new ErrorInfo(ErrorType.RateLimitOrder, false, "Too many open orders", code);

                    if (msg!.Equals("Rate limit exceeded") || msg.Equals("Domain rate limit exceeded"))
                        return new ErrorInfo(ErrorType.RateLimitRequest, false, "Too many requests", code);

                    if (msg!.Equals("Reduce only:No position exists"))
                        return new ErrorInfo(ErrorType.NoPosition, false, "No position for reduce only order", code);

                    if (msg!.Equals("Reduce only:Position is closed"))
                        return new ErrorInfo(ErrorType.NoPosition, false, "Position has been closed, remaining quantity is canceled", code);

                    if (msg!.Equals("Scheduled orders limit exceeded"))
                        return new ErrorInfo(ErrorType.RateLimitOrder, false, "Too many scheduled orders", code);

                    if (msg!.Equals("Unknown order"))
                        return new ErrorInfo(ErrorType.UnknownOrder, false, "Unknown order", code);

                    if (msg!.Equals("Margin position size exceeded")
                     || msg!.Equals("Positions limit exceeded"))
                    {
                        return new ErrorInfo(ErrorType.MaxPosition, false, "Max position size exceeded", code);
                    }

                    return new ErrorInfo(ErrorType.Unknown, false, msg!, code);
                }),
            new ErrorEvaluator("EService", (code, msg) => {
                    if(string.IsNullOrEmpty(msg))
                        return new ErrorInfo(ErrorType.Unknown, false, "Unknown error", code);

                    if (msg!.Equals("Unavailable"))
                        return new ErrorInfo(ErrorType.SystemError, true, "System offline", code);

                    if (msg!.Equals("Busy"))
                        return new ErrorInfo(ErrorType.SystemError, true, "System busy", code);

                    if (msg!.Equals("Market in cancel_only mode"))
                        return new ErrorInfo(ErrorType.RejectedOrderConfiguration, false, "Symbol in cancel only mode", code);

                    if (msg!.Equals("Market in post_only  mode"))
                        return new ErrorInfo(ErrorType.RejectedOrderConfiguration, false, "Symbol in post only mode", code);

                    if (msg!.Equals("Deadline elapsed  mode"))
                        return new ErrorInfo(ErrorType.RejectedOrderConfiguration, false, "Order deadline passed", code);

                    return new ErrorInfo(ErrorType.Unknown, false, msg!, code);
                }),
            new ErrorEvaluator("Subscription", (code, msg) => {
                    if(string.IsNullOrEmpty(msg))
                        return new ErrorInfo(ErrorType.Unknown, false, "Unknown error", code);

                    if (msg!.StartsWith("Currency pair not supported"))
                        return new ErrorInfo(ErrorType.UnknownSymbol, false, "Unknown symbol", code);

                    if (msg!.StartsWith("Currency pair not in "))
                        return new ErrorInfo(ErrorType.UnknownSymbol, false, "Symbol format invalid", code);

                    return new ErrorInfo(ErrorType.Unknown, false, msg!, code);
                }),
            ]);

        public static ErrorMapping FuturesMapping = new ErrorMapping([
            new ErrorInfo(ErrorType.Unauthorized, false, "Account inactive", "accountInactive"),
            new ErrorInfo(ErrorType.Unauthorized, false, "Authentication error", "authenticationError"),

            new ErrorInfo(ErrorType.InvalidTimestamp, false, "Invalid timestamp/nonce", "nonceDuplicate", "nonceBelowThreshold"),

            new ErrorInfo(ErrorType.RateLimitRequest, false, "Too many requests", "apiLimitExceeded"),

            new ErrorInfo(ErrorType.UnknownSymbol, false, "Unknown symbol", "contractNotFound"),

            new ErrorInfo(ErrorType.InvalidParameter, false, "Invalid parameter", "invalidArgument"),
            new ErrorInfo(ErrorType.InvalidParameter, false, "Client order id too long", "clientOrderIdTooBig"),
            new ErrorInfo(ErrorType.InvalidParameter, false, "Client order id invalid", "ClientOrderIdInvalid"),

            new ErrorInfo(ErrorType.MissingParameter, false, "Missing parameter", "requiredArgumentMissing"),

            new ErrorInfo(ErrorType.InvalidQuantity, false, "Invalid quantity", "invalidAmount", "invalidSize"),

            new ErrorInfo(ErrorType.InvalidPrice, false, "Invalid price", "invalidPrice"),
            new ErrorInfo(ErrorType.InvalidPrice, false, "Invalid price", "invalidArgument: limitPrice"),

            new ErrorInfo(ErrorType.DuplicateClientOrderId, false, "Duplicate clientOrderId", "clientOrderIdAlreadyExist"),

            new ErrorInfo(ErrorType.InsufficientBalance, false, "Insufficient balance", "insufficientFunds", "insufficientAvailableFunds"),

            new ErrorInfo(ErrorType.UnavailableSymbol, false, "Symbol currently unavailable", "marketUnavailable"),
            new ErrorInfo(ErrorType.UnavailableSymbol, false, "Symbol currently suspended", "marketSuspended"),
            new ErrorInfo(ErrorType.UnavailableSymbol, false, "Symbol inactive", "marketInactive"),

            new ErrorInfo(ErrorType.RejectedOrderConfiguration, false, "PostOnly order would immediately execute", "postWouldExecute"),
            new ErrorInfo(ErrorType.RejectedOrderConfiguration, false, "ImmediateOrCancel order would not immediately execute", "iocWouldNotExecute"),
            new ErrorInfo(ErrorType.RejectedOrderConfiguration, false, "Self fill prevented", "selfFill"),
            new ErrorInfo(ErrorType.RejectedOrderConfiguration, false, "Market is in post only mode", "marketIsPostOnly"),
            new ErrorInfo(ErrorType.RejectedOrderConfiguration, false, "Reduce only order would not reduce position", "wouldNotReducePosition"),

            new ErrorInfo(ErrorType.RateLimitOrder, false, "Too many open orders", "tooManyOrders"),

            ]);

    }
}
