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

        #region Get Deposit Addresses

        async Task<ICallResult<SharedDepositAddress[]>> IGetDepositAddresses.GetDepositAddressesAsync(GetDepositAddressesRequest request, CancellationToken ct)
            => await GetDepositAddressesAsync(request, ct).ConfigureAwait(false);

        public GetDepositAddressesOptions GetDepositAddressesOptions { get; } = new GetDepositAddressesOptions(_exchangeName, true)
        {
            ParameterRuleOverwrites = [
                RequestParameterRuleOverride<GetDepositAddressesRequest>.Required(x => x.Network)
                ]
        };
        public async Task<HttpResult<SharedDepositAddress[]>> GetDepositAddressesAsync(GetDepositAddressesRequest request, CancellationToken ct)
        {
            var validationError = GetDepositAddressesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedDepositAddress[]>(Exchange, validationError);

            var depositAddresses = await _api.Account.GetDepositAddressesAsync(KrakenExchange.AssetAliases.CommonToExchangeName(request.Asset), request.Network!).ConfigureAwait(false);
            if (!depositAddresses.Success)
                return HttpResult.Fail<SharedDepositAddress[]>(depositAddresses);

            return HttpResult.Ok(depositAddresses, depositAddresses.Data.Select(x => new SharedDepositAddress(request.Asset, x.Address)
            {
                TagOrMemo = x.Tag
            }
            ).ToArray());
        }

        #endregion

        #region Get Deposit History

        async Task<ICallResult<SharedDeposit[]>> IGetDepositHistory.GetDepositHistoryAsync(GetDepositsRequest request, PageRequest? pageRequest, CancellationToken ct)
            => await GetDepositHistoryAsync(request, pageRequest, ct).ConfigureAwait(false);

        Task<HttpResult<SharedDeposit[]>> IDepositRestClient.GetDepositsAsync(GetDepositsRequest request, PageRequest? nextPageToken, CancellationToken ct)
            => GetDepositHistoryAsync(request, nextPageToken, ct);
        GetDepositHistoryOptions IDepositRestClient.GetDepositsOptions => GetDepositHistoryOptions;

        public GetDepositHistoryOptions GetDepositHistoryOptions { get; } = new GetDepositHistoryOptions(_exchangeName, false, true, true, 100);
        public async Task<HttpResult<SharedDeposit[]>> GetDepositHistoryAsync(GetDepositsRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetDepositHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedDeposit[]>(Exchange, validationError);

            int limit = request.Limit ?? 100;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var result = await _api.Account.GetDepositHistoryAsync(
                asset: request.Asset != null ? KrakenExchange.AssetAliases.CommonToExchangeName(request.Asset) : null,
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                limit: pageParams.Limit,
                cursor: pageParams.Cursor,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedDeposit[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                     () => result.Data.NextCursor == null ? null : Pagination.NextPageFromCursor(result.Data.NextCursor),
                     result.Data.Items.Length,
                     result.Data.Items.Select(x => x.Timestamp),
                     request.StartTime,
                     request.EndTime ?? DateTime.UtcNow,
                     pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data.Items, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                    .Select(x =>
                        new SharedDeposit(
                            KrakenExchange.AssetAliases.ExchangeToCommonName(x.Asset),
                            x.Quantity,
                            true,
                            x.Timestamp,
                            x.Status == "SUCCESS" ? SharedTransferStatus.Completed
                            : x.Status == "FAILURE" ? SharedTransferStatus.Failed
                            : SharedTransferStatus.Unknown)
                        {
                            Id = x.ReferenceId,
                            TransactionId = x.TransactionId,
                            Network = x.Method
                        })
                    .ToArray(), nextPageRequest);
        }

        #endregion

    }
}
