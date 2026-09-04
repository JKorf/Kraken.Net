using CryptoExchange.Net.SharedApis;
using Kraken.Net.Enums;
using Kraken.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Net.Clients.SpotApi
{
    internal partial class KrakenRestClientSpotSharedApi
    {

        public SharedSymbolCatalog? SpotSymbolCatalog => ExchangeSymbolCache.GetSymbolCatalog(_exchangeName, _topicId, _api.EnvironmentName, null);

        #region Get Spot Symbols

        async Task<ICallResult<SharedSpotSymbol[]>> IGetSpotSymbols.GetSpotSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
            => await GetSpotSymbolsAsync(request, ct).ConfigureAwait(false);

        public GetSpotSymbolsOptions GetSpotSymbolsOptions { get; } = new GetSpotSymbolsOptions(_exchangeName, false)
        {
            OptionalExchangeParameters = [
                ExchangeParameterDescription.Optional<bool>(
                    "NewAssetNames",
                    aliases: ["assetVersion"],
                    description: "If true, the response will use the new asset names (e.g. instead of XBT, BTC will be used)",
                    exampleValue: false)
                ]
        };
        public async Task<HttpResult<SharedSpotSymbol[]>> GetSpotSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = GetSpotSymbolsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotSymbol[]>(Exchange, validationError);

            var useNewAssetResponse = request.GetParamValue<bool?>(Exchange, "NewAssetNames", "assetVersion");
            var currencyTask = _api.ExchangeData.GetSymbolsAsync(newAssetNameResponse: useNewAssetResponse, aClass: AClass.Currency, ct: ct);
            var tokenTask = _api.ExchangeData.GetSymbolsAsync(newAssetNameResponse: useNewAssetResponse, aClass: AClass.TokenizedAsset, ct: ct);
            await Task.WhenAll(currencyTask, tokenTask).ConfigureAwait(false);
            var currencyResult = currencyTask.Result;
            var tokenResult = tokenTask.Result;
            if (!currencyResult.Success)
                return HttpResult.Fail<SharedSpotSymbol[]>(currencyResult);
            if (!tokenResult.Success)
                return HttpResult.Fail<SharedSpotSymbol[]>(tokenResult);

            var currencyResultData = currencyResult.Data
               .Select(x => ParseSymbol(x, false));
            var tokenResultData = tokenResult.Data.Where(x => !x.Key.EndsWith("SPVUSD"))
               .Select(x => ParseSymbol(x, true));
            var resultData = currencyResultData.Concat(tokenResultData)
               .ToArray();

            // Also register [BaseAsset]/[QuoteAsset] and [BaseAsset][QuoteAsset] as names
            var symbolRegistrations = resultData
                .Concat(resultData.Select(x => new SharedSpotSymbol(x.BaseAsset, x.QuoteAsset, x.BaseAsset + "/" + x.QuoteAsset, x.Trading, x.TradingMode)))
                .Concat(resultData.Where(x => x.Name != x.BaseAsset + x.QuoteAsset).Select(x => new SharedSpotSymbol(x.BaseAsset, x.QuoteAsset, x.BaseAsset + x.QuoteAsset, x.Trading, x.TradingMode)))
                .ToArray();
            ExchangeSymbolCache.UpdateSymbolInfo(_topicId, _api.EnvironmentName, null, symbolRegistrations);
            return HttpResult.Ok(currencyResult, SharedUtils.ApplySymbolFilter(resultData, request));
        }

        #endregion

        private SharedSpotSymbol ParseSymbol(KeyValuePair<string, KrakenSymbol> s, bool isTokenized)
        {
            var assets = GetAssets(s.Value.WebsocketName);
            var result = new SharedSpotSymbol(assets.BaseAsset, assets.QuoteAsset, s.Key, s.Value.Status == SymbolStatus.Online)
            {
                PriceDecimals = s.Value.PriceDecimals,
                QuantityDecimals = s.Value.LotDecimals,
                MinTradeQuantity = s.Value.OrderMin,
                PriceStep = s.Value.TickSize,
                MinNotionalValue = s.Value.MinValue,
                DisplayName = s.Key,
                BaseAssetType = isTokenized ? SharedAssetType.TradFi : SharedAssetType.Crypto,
                MakerFeePercentage = s.Value.FeesMaker.FirstOrDefault()?.FeePercentage,
                TakerFeePercentage = s.Value.Fees.FirstOrDefault()?.FeePercentage,
            };

            if (LibraryHelpers.IsStableCoin(result.QuoteAsset))
            {
                result.QuoteAssetType = SharedAssetType.Crypto;
                result.QuoteAssetSubType = SharedAssetSubType.StableCoin;
            }
            else if (_exchangeFiat.Contains(result.QuoteAsset))
            {
                result.QuoteAssetType = SharedAssetType.Fiat;
            }
            else
            {
                result.QuoteAssetType = SharedAssetType.Crypto;
            }

            if (isTokenized)
            {
                result.BaseAssetSubType = SharedAssetSubType.Equity;
            }
            else
            {
                if (LibraryHelpers.IsStableCoin(result.BaseAsset))
                    result.BaseAssetSubType = SharedAssetSubType.StableCoin;
            }

            return result;
        }

        private (string BaseAsset, string QuoteAsset) GetAssets(string name)
        {
            var split = name.Split('/');
            return (KrakenExchange.AssetAliases.ExchangeToCommonName(split[0]), KrakenExchange.AssetAliases.ExchangeToCommonName(split[1]));
        }

        public async Task<ExchangeCallResult<SharedSymbol[]>> GetSpotSymbolsForBaseAssetAsync(string baseAsset)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetSpotSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<SharedSymbol[]>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<SharedSymbol[]>.Ok(Exchange, ExchangeSymbolCache.GetSymbolsForBaseAsset(_topicId, _api.EnvironmentName, null, baseAsset));
        }

        public async Task<ExchangeCallResult<bool>> SupportsSpotSymbolAsync(SharedSymbol symbol)
        {
            if (symbol.TradingMode != TradingMode.Spot)
                throw new ArgumentException(nameof(symbol), "Only Spot symbols allowed");

            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetSpotSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, _api.EnvironmentName, null, symbol));
        }

        public async Task<ExchangeCallResult<bool>> SupportsSpotSymbolAsync(string symbolName)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetSpotSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, _api.EnvironmentName, null, symbolName));
        }
    }
}
