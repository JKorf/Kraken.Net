using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using Kraken.Net.Clients;
using Kraken.Net.Objects.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kraken.Net.UnitTests
{
    [TestFixture]
    public class RestRequestTests
    {
        [Test]
        public async Task ValidateSpotAccountCalls()
        {
            var client = new KrakenRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new ApiCredentials("MTIz", "MTIz");
                opts.RateLimiterEnabled = false;
            });
            var tester = new RestRequestValidator<KrakenRestClient>(client, "Endpoints/Spot/Account", "https://api.kraken.com", IsAuthenticatedSpot, "result");
            await tester.ValidateAsync(client => client.SpotApi.Account.GetBalancesAsync(), "GetBalances");
            await tester.ValidateAsync(client => client.SpotApi.Account.GetAvailableBalancesAsync(), "GetAvailableBalances");
            await tester.ValidateAsync(client => client.SpotApi.Account.GetTradeBalanceAsync(), "GetTradeBalance");
            await tester.ValidateAsync(client => client.SpotApi.Account.GetOpenPositionsAsync(), "GetOpenPositions");
            await tester.ValidateAsync(client => client.SpotApi.Account.GetLedgerInfoAsync(), "GetLedgerInfo", skipResponseValidation: true);
            await tester.ValidateAsync(client => client.SpotApi.Account.GetLedgersEntryAsync(), "GetLedgersEntry");
            await tester.ValidateAsync(client => client.SpotApi.Account.GetTradeVolumeAsync(), "GetTradeVolume", skipResponseValidation: true);
            await tester.ValidateAsync(client => client.SpotApi.Account.GetDepositMethodsAsync("ETH"), "GetDepositMethods", ignoreProperties: new List<string> { "limit" });
            await tester.ValidateAsync(client => client.SpotApi.Account.GetDepositAddressesAsync("ETH", "123"), "GetDepositAddresses");
            await tester.ValidateAsync(client => client.SpotApi.Account.GetDepositStatusAsync(), "GetDepositStatus");
            await tester.ValidateAsync(client => client.SpotApi.Account.GetWithdrawInfoAsync("ETH", "123", 1), "GetWithdrawInfo");
            await tester.ValidateAsync(client => client.SpotApi.Account.WithdrawAsync("ETH", "123", 1), "Withdraw");
            await tester.ValidateAsync(client => client.SpotApi.Account.GetWithdrawAddressesAsync("ETH", Enums.AClass.TokenizedAsset, "123"), "GetWithdrawAddresses");
            await tester.ValidateAsync(client => client.SpotApi.Account.GetWithdrawMethodsAsync("ETH", Enums.AClass.TokenizedAsset, "123"), "GetWithdrawMethods");
            await tester.ValidateAsync(client => client.SpotApi.Account.GetWithdrawalStatusAsync("ETH", "123"), "GetWithdrawalStatus");
            await tester.ValidateAsync(client => client.SpotApi.Account.CancelWithdrawalAsync("ETH", "123"), "CancelWithdrawal");
            await tester.ValidateAsync(client => client.SpotApi.Account.TransferAsync("ETH", 1, "123", "123"), "Transfer");
        }

        [Test]
        public async Task ValidateSpotExchangeDataCalls()
        {
            var client = new KrakenRestClient(opts =>
            {
                opts.AutoTimestamp = false;
            });
            var tester = new RestRequestValidator<KrakenRestClient>(client, "Endpoints/Spot/ExchangeData", "https://api.kraken.com", IsAuthenticatedSpot, "result");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetSystemStatusAsync(), "GetSystemStatus");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetAssetsAsync(), "GetAssets");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetSymbolsAsync(), "GetSymbols");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetTickerAsync("ETHUSDT"), "GetTicker");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetKlinesAsync("ETHUSDT", Enums.KlineInterval.OneDay), "GetKlines", skipResponseValidation: true);
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetOrderBookAsync("ETHUSDT"), "GetOrderBook", skipResponseValidation: true);
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetTradeHistoryAsync("ETHUSDT"), "GetTradeHistory", skipResponseValidation: true);
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetRecentSpreadAsync("ETHUSDT"), "GetRecentSpread", skipResponseValidation: true);
        }

        [Test]
        public async Task ValidateSpotTradingCalls()
        {
            var client = new KrakenRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new ApiCredentials("MTIz", "MTIz");
                opts.RateLimiterEnabled = false;
            });
            var tester = new RestRequestValidator<KrakenRestClient>(client, "Endpoints/Spot/Trading", "https://api.kraken.com", IsAuthenticatedSpot, "result");
            //await tester.ValidateAsync(client => client.SpotApi.Trading.GetOpenOrdersAsync(), "GetOpenOrders", skipResponseValidation: true);
            //await tester.ValidateAsync(client => client.SpotApi.Trading.GetClosedOrdersAsync(), "GetClosedOrders", skipResponseValidation: true);
            //await tester.ValidateAsync(client => client.SpotApi.Trading.GetOrdersAsync(), "GetOrders", skipResponseValidation: true);
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetOrdersAsync(), "GetOrders2", ignoreProperties: ["aclass", "price"]);
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetUserTradesAsync(), "GetUserTrades", skipResponseValidation: true);
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetUserTradeDetailsAsync("123"), "GetUserTradeDetails", skipResponseValidation: true);
            await tester.ValidateAsync(client => client.SpotApi.Trading.PlaceMultipleOrdersAsync("ETHUSDT", new[] { new KrakenOrderRequest() } ), "PlaceMultipleOrders", skipResponseValidation: true);
            await tester.ValidateAsync(client => client.SpotApi.Trading.PlaceOrderAsync("ETHUSDT", Enums.OrderSide.Sell, Enums.OrderType.Market, 1 ), "PlaceOrder");
            await tester.ValidateAsync(client => client.SpotApi.Trading.EditOrderAsync("ETHUSDT", "123", 1), "EditOrder");
            await tester.ValidateAsync(client => client.SpotApi.Trading.CancelOrderAsync("ETHUSDT", "123"), "CancelOrder");
            await tester.ValidateAsync(client => client.SpotApi.Trading.CancelAllOrdersAsync("ETHUSDT"), "CancelAllOrders");
            await tester.ValidateAsync(client => client.SpotApi.Trading.CancelAllOrdersAfterAsync(TimeSpan.FromSeconds(5)), "CancelAllOrdersAfter");
            await tester.ValidateAsync(client => client.SpotApi.Trading.CancelMultipleOrdersAsync(["123", "456"]), "CancelMultipleOrders");
        }

        [Test]
        public async Task ValidateSpotEarnCalls()
        {
            var client = new KrakenRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new ApiCredentials("MTIz", "MTIz");
                opts.RateLimiterEnabled = false;
            });
            var tester = new RestRequestValidator<KrakenRestClient>(client, "Endpoints/Spot/Earn", "https://api.kraken.com", IsAuthenticatedSpot, "result");
            await tester.ValidateAsync(client => client.SpotApi.Earn.GetStrategiesAsync(), "GetStrategies");
            await tester.ValidateAsync(client => client.SpotApi.Earn.GetAllocationsAsync(), "GetAllocations");
            await tester.ValidateAsync(client => client.SpotApi.Earn.GetAllocationStatusAsync("123"), "GetAllocationStatus");
            await tester.ValidateAsync(client => client.SpotApi.Earn.GetDeallocationStatusAsync("123"), "GetDeallocationStatus");
            await tester.ValidateAsync(client => client.SpotApi.Earn.AllocateEarnFundsAsync("123", 1), "AllocateEarnFunds");
            await tester.ValidateAsync(client => client.SpotApi.Earn.DeallocateEarnFundsAsync("123", 1), "DeallocateEarnFunds");
        }

        [Test]
        public async Task ValidateFuturesAccountCalls()
        {
            var client = new KrakenRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.RateLimiterEnabled = false;
                opts.ApiCredentials = new ApiCredentials("MTIz", "MTIz");
            });
            var tester = new RestRequestValidator<KrakenRestClient>(client, "Endpoints/Futures/Account", "https://futures.kraken.com", IsAuthenticatedFutures);
            await tester.ValidateAsync(client => client.FuturesApi.Account.GetBalancesAsync(), "GetBalances", "result", skipResponseValidation: true);
            await tester.ValidateAsync(client => client.FuturesApi.Account.GetPnlCurrencyAsync(), "GetPnlCurrency", "result");
            await tester.ValidateAsync(client => client.FuturesApi.Account.SetPnlCurrencyAsync("ETHUSDT", "ETH"), "SetPnlCurrency");
            await tester.ValidateAsync(client => client.FuturesApi.Account.TransferAsync("ETH", 1, "1", "2"), "Transfer");
            await tester.ValidateAsync(client => client.FuturesApi.Account.GetAccountLogAsync(), "GetAccountLog");
            await tester.ValidateAsync(client => client.FuturesApi.Account.GetFeeScheduleVolumeAsync(), "GetFeeScheduleVolume", "result", skipResponseValidation: true);
            await tester.ValidateAsync(client => client.FuturesApi.Account.GetInitialMarginRequirementsAsync("ETHUSDT", Enums.FuturesOrderType.Limit, Enums.OrderSide.Buy, 1), "GetInitialMarginRequirements", skipResponseValidation: true);
            await tester.ValidateAsync(client => client.FuturesApi.Account.GetMaxOrderQuantityAsync("ETHUSDT", Enums.FuturesOrderType.TakeProfit), "GetMaxOrderQuantity", skipResponseValidation: true);
        }

        [Test]
        public async Task ValidateFuturesTradingCalls()
        {
            var client = new KrakenRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.RateLimiterEnabled = false;
                opts.ApiCredentials = new ApiCredentials("MTIz", "MTIz");
            });
            var tester = new RestRequestValidator<KrakenRestClient>(client, "Endpoints/Futures/Trading", "https://futures.kraken.com", IsAuthenticatedFutures);
            await tester.ValidateAsync(client => client.FuturesApi.Trading.GetUserTradesAsync(), "GetUserTrades", "result");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.GetSelfTradeStrategyAsync(), "GetSelfTradeStrategy", skipResponseValidation: true);
            await tester.ValidateAsync(client => client.FuturesApi.Trading.SetSelfTradeStrategyAsync(Enums.SelfTradeStrategy.CancelMakerSelf), "SetSelfTradeStrategy", skipResponseValidation: true);
            await tester.ValidateAsync(client => client.FuturesApi.Trading.GetOpenPositionsAsync(), "GetOpenPositions", "openPositions");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.GetLeverageAsync(), "GetLeverage", "leveragePreferences");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.SetLeverageAsync("ETHUSDT", 1), "SetLeverage");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.PlaceOrderAsync("ETHUSDT", Enums.OrderSide.Buy, Enums.FuturesOrderType.Market, 1), "PlaceOrder", "sendStatus");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.GetOpenOrdersAsync(), "GetOpenOrders", "openOrders");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.GetOrdersAsync(), "GetOrders", "openOrders", skipResponseValidation: true);
            await tester.ValidateAsync(client => client.FuturesApi.Trading.EditOrderAsync(), "EditOrder", "editStatus", ignoreProperties: new List<string> { "orderId" });
            await tester.ValidateAsync(client => client.FuturesApi.Trading.CancelOrderAsync(), "CancelOrder", "cancelStatus");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.CancelAllOrdersAsync(), "CancelAllOrders", "cancelStatus");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.CancelAllOrderAfterAsync(TimeSpan.Zero), "CancelAllOrderAfter", "status");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.GetExecutionEventsAsync(), "GetExecutionEvents"); 
        }

        [Test]
        public async Task ValidateFuturesExchangeDataCalls()
        {
            var client = new KrakenRestClient(opts =>
            {
                opts.RateLimiterEnabled = false;
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new ApiCredentials("MTIz", "MTIz");
            });
            var tester = new RestRequestValidator<KrakenRestClient>(client, "Endpoints/Futures/ExchangeData", "https://futures.kraken.com", IsAuthenticatedFutures);
            await tester.ValidateAsync(client => client.FuturesApi.ExchangeData.GetPlatformNotificationsAsync(), "GetPlatformNotifications", "result");
            await tester.ValidateAsync(client => client.FuturesApi.ExchangeData.GetHistoricalFundingRatesAsync("ETHUSDT"), "GetHistoricalFundingRates", "result");
            await tester.ValidateAsync(client => client.FuturesApi.ExchangeData.GetFeeSchedulesAsync(), "GetFeeSchedules", "result");
            await tester.ValidateAsync(client => client.FuturesApi.ExchangeData.GetSymbolsAsync(), "GetSymbols", "result");
            await tester.ValidateAsync(client => client.FuturesApi.ExchangeData.GetSymbolStatusAsync(), "GetSymbolStatus", "result");
            await tester.ValidateAsync(client => client.FuturesApi.ExchangeData.GetTradesAsync("ETHUSDT"), "GetTrades", "result");
            await tester.ValidateAsync(client => client.FuturesApi.ExchangeData.GetOrderBookAsync("ETHUSDT"), "GetOrderBook", "result");
            await tester.ValidateAsync(client => client.FuturesApi.ExchangeData.GetTickersAsync(), "GetTickers", "result");
            await tester.ValidateAsync(client => client.FuturesApi.ExchangeData.GetKlinesAsync(Enums.TickType.Mark, "ETHUSDT", Enums.FuturesKlineInterval.TwelfHours), "GetKlines");
        }

        private bool IsAuthenticatedSpot(WebCallResult result)
        {
            return result.RequestHeaders.Any(h => h.Key == "API-Sign");
        }

        private bool IsAuthenticatedFutures(WebCallResult result)
        {
            return result.RequestHeaders.Any(h => h.Key == "Authent");
        }
    }
}
