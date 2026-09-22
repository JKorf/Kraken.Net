using CryptoExchange.Net.SharedApis;
using Kraken.Net.Objects.Models.Futures;

namespace Kraken.Net.Clients.FuturesApi
{
	internal partial class KrakenRestClientFuturesSharedApi
    {

        public SharedSymbolCatalog? FuturesSymbolCatalog => ExchangeSymbolCache.GetSymbolCatalog(_exchangeName, _topicId, _api.EnvironmentName, null);

        #region Get Futures Symbols

        async Task<IExchangeCallResult<SharedFuturesSymbol[]>> IGetFuturesSymbols.GetFuturesSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
            => await GetFuturesSymbolsAsync(request, ct).ConfigureAwait(false);

        public GetFuturesSymbolsOptions GetFuturesSymbolsOptions { get; } = new GetFuturesSymbolsOptions(_exchangeName, false);
        public async Task<HttpResult<SharedFuturesSymbol[]>> GetFuturesSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = GetFuturesSymbolsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesSymbol[]>(Exchange, validationError);

            var result = await _api.ExchangeData.GetSymbolsAsync(ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedFuturesSymbol[]>(result);

            var data = result.Data
               .Where(x => x.Type != Enums.SymbolType.SpotIndex && !x.Symbol.StartsWith("rr"))
               .Select(x => ParseSymbol(x))
               .ToArray();

            ExchangeSymbolCache.UpdateSymbolInfo(_topicId, _api.EnvironmentName, null, data);
            return HttpResult.Ok(result, SharedUtils.ApplySymbolFilter(data, request));
        }

        #endregion

        private SharedFuturesSymbol ParseSymbol(KrakenFuturesSymbol s)
        {
            var split = s.Symbol.Split('_');
            var assets = split[1];

            var result = new SharedFuturesSymbol(
                s.Type == Enums.SymbolType.FlexibleFutures && split.Count() == 2 ? TradingMode.PerpetualLinear :
                s.Type == Enums.SymbolType.FlexibleFutures && split.Count() > 2 ? TradingMode.DeliveryLinear :
                s.Type == Enums.SymbolType.InverseFutures && split.Count() == 2 ? TradingMode.PerpetualInverse :
                TradingMode.DeliveryInverse,
                s.BaseAsset,
                s.QuoteAsset,
                s.Symbol,
                s.Tradeable)
            {
                QuantityDecimals = (int?)s.ContractValueTradePrecision,
                PriceStep = s.TickSize,
                ContractSize = s.ContractSize,
                DeliveryTime = split.Count() > 2 ? DateTime.ParseExact(split[2], "yyMMdd", CultureInfo.InvariantCulture) : null,
                DisplayName = s.Symbol,
                QuoteAssetType = SharedAssetType.Fiat,
                BaseAssetType = SharedAssetType.Crypto
            };

            return result;
        }

        public async Task<ExchangeCallResult<SharedSymbol[]>> GetFuturesSymbolsForBaseAssetAsync(string baseAsset)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetFuturesSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<SharedSymbol[]>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<SharedSymbol[]>.Ok(Exchange, ExchangeSymbolCache.GetSymbolsForBaseAsset(_topicId, _api.EnvironmentName, null, baseAsset));
        }

        public async Task<ExchangeCallResult<bool>> SupportsFuturesSymbolAsync(SharedSymbol symbol)
        {
            if (symbol.TradingMode == TradingMode.Spot)
                throw new ArgumentException(nameof(symbol), "Spot symbols not allowed");

            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetFuturesSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, _api.EnvironmentName, null, symbol));
        }

        public async Task<ExchangeCallResult<bool>> SupportsFuturesSymbolAsync(string symbolName)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetFuturesSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, _api.EnvironmentName, null, symbolName));
        }
    }
}
