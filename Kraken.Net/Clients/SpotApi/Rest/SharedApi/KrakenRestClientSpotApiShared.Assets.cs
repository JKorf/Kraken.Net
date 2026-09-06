using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Net.Clients.SpotApi
{
    internal partial class KrakenRestClientSpotSharedApi
    {

        #region Get Asset

        async Task<ICallResult<SharedAsset>> IGetAsset.GetAssetAsync(GetAssetRequest request, CancellationToken ct)
            => await GetAssetAsync(request, ct).ConfigureAwait(false);

        public GetAssetOptions GetAssetOptions { get; } = new GetAssetOptions(_exchangeName, true);
        public async Task<HttpResult<SharedAsset>> GetAssetAsync(GetAssetRequest request, CancellationToken ct)
        {
            var validationError = GetAssetOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedAsset>(Exchange, validationError);

            var assets = await _api.Account.GetWithdrawMethodsAsync(KrakenExchange.AssetAliases.ExchangeToCommonName(request.Asset), ct: ct).ConfigureAwait(false);
            if (!assets.Success)
                return HttpResult.Fail<SharedAsset>(assets);

            if (!assets.Data.Any())
                return HttpResult.Fail<SharedAsset>(assets, new ServerError(new ErrorInfo(ErrorType.UnknownAsset, "Asset not found")));

            return HttpResult.Ok(assets, new SharedAsset(KrakenExchange.AssetAliases.ExchangeToCommonName(request.Asset))
            {
                Networks = assets.Data.Select(x => new SharedAssetNetwork(x.Network)
                {
                    FullName = x.Method,
                    MinWithdrawQuantity = x.Minimum
                }).ToArray()
            });
        }

        #endregion

        #region Get All Assets

        async Task<ICallResult<SharedAsset[]>> IGetAllAssets.GetAllAssetsAsync(GetAssetsRequest request, CancellationToken ct)
            => await GetAllAssetsAsync(request, ct).ConfigureAwait(false);

        Task<HttpResult<SharedAsset[]>> IAssetsRestClient.GetAssetsAsync(GetAssetsRequest request, CancellationToken ct)
            => GetAllAssetsAsync(request, ct);
        GetAllAssetsOptions IAssetsRestClient.GetAssetsOptions => GetAllAssetsOptions;

        public GetAllAssetsOptions GetAllAssetsOptions { get; } = new GetAllAssetsOptions(_exchangeName, false)
        {
            RequestNotes = "If API credentials are set and the NewAssetNames Exchange Parameter is not set to true then withdrawal networks will also be returned",
            OptionalExchangeParameters = [
                ExchangeParameterRule.Optional<bool>(
                    "NewAssetNames",
                    aliases: ["assetVersion"],
                    description: "If true, the response will use the new asset names (e.g. instead of XBT, BTC will be used)",
                    exampleValue: false)
                ]
        };
        public async Task<HttpResult<SharedAsset[]>> GetAllAssetsAsync(GetAssetsRequest request, CancellationToken ct)
        {
            var validationError = GetAllAssetsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedAsset[]>(Exchange, validationError);

            var useNewAssetResponse = request.GetParamValue<bool?>(Exchange, "NewAssetNames", "assetVersion");

            if (Authenticated && useNewAssetResponse != true)
            {
                var assets = await _api.Account.GetWithdrawMethodsAsync(ct: ct).ConfigureAwait(false);
                if (!assets.Success)
                    return HttpResult.Fail<SharedAsset[]>(assets);

                return HttpResult.Ok(assets, assets.Data.GroupBy(x => KrakenExchange.AssetAliases.ExchangeToCommonName(x.Asset)).Select(x => new SharedAsset(x.Key)
                {
                    Networks = x.Select(x => new SharedAssetNetwork(x.Network)
                    {
                        FullName = x.Method,
                        MinWithdrawQuantity = x.Minimum
                    }).ToArray()
                }).ToArray());
            }
            else
            {
                var assets = await _api.ExchangeData.GetAssetsAsync(newAssetNameResponse: useNewAssetResponse, ct: ct).ConfigureAwait(false);
                if (!assets.Success)
                    return HttpResult.Fail<SharedAsset[]>(assets);

                return HttpResult.Ok(assets, assets.Data.Select(x => new SharedAsset(KrakenExchange.AssetAliases.ExchangeToCommonName(x.Key))
                {
                    FullName = x.Value.AlternateName
                }).ToArray());
            }
        }

        #endregion

    }
}
