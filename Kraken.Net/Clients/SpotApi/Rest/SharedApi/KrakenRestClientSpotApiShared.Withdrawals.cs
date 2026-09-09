using CryptoExchange.Net.SharedApis;
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

        #region Get Withdrawal History

        async Task<ICallResult<SharedWithdrawal[]>> IGetWithdrawalHistory.GetWithdrawalHistoryAsync(GetWithdrawalsRequest request, PageRequest? pageRequest, CancellationToken ct)
            => await GetWithdrawalHistoryAsync(request, pageRequest, ct).ConfigureAwait(false);

        Task<HttpResult<SharedWithdrawal[]>> IWithdrawalRestClient.GetWithdrawalsAsync(GetWithdrawalsRequest request, PageRequest? nextPageToken, CancellationToken ct)
            => GetWithdrawalHistoryAsync(request, nextPageToken, ct);
        GetWithdrawalHistoryOptions IWithdrawalRestClient.GetWithdrawalsOptions => GetWithdrawalHistoryOptions;

        public GetWithdrawalHistoryOptions GetWithdrawalHistoryOptions { get; } = new GetWithdrawalHistoryOptions(_exchangeName, false, true, true, 500)
        {
            RequestNotes = "The Address field contains the label of the saved withdrawal address, not the actual address"
        };
        public async Task<HttpResult<SharedWithdrawal[]>> GetWithdrawalHistoryAsync(GetWithdrawalsRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetWithdrawalHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedWithdrawal[]>(Exchange, validationError);

            int limit = request.Limit ?? 100;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var result = await _api.Account.GetWithdrawalHistoryAsync(
                asset: request.Asset != null ? KrakenExchange.AssetAliases.CommonToExchangeName(request.Asset) : null,
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                limit: pageParams.Limit,
                cursor: pageParams.Cursor,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedWithdrawal[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                     () => result.Data.NextCursor == null ? null : Pagination.NextPageFromCursor(result.Data.NextCursor),
                     result.Data.Items.Length,
                     result.Data.Items.Select(x => x.Timestamp),
                     request.StartTime,
                     request.EndTime ?? DateTime.UtcNow,
                     pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data.Items, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                    .Select(x =>
                        new SharedWithdrawal(
                            KrakenExchange.AssetAliases.ExchangeToCommonName(x.Asset),
                            x.Key ?? string.Empty,
                            x.Quantity,
                            x.Status == "Success",
                            x.Timestamp,
                            GetWithdrawalStatus(x))
                        {
                            Id = x.ReferenceId,
                            TransactionId = x.TransactionId,
                            Network = x.Method,
                            Fee = x.Fee
                        })
                    .ToArray(), nextPageRequest);
        }

        #endregion

        private SharedTransferStatus GetWithdrawalStatus(KrakenMovementStatus x)
        {
            if (x.Status == "Failure")
                return SharedTransferStatus.Failed;

            if (x.Status == "Success")
                return SharedTransferStatus.Completed;

            if (x.Status == "Initial" || x.Status == "Pending" || x.Status == "Settled")
                return SharedTransferStatus.InProgress;

            return SharedTransferStatus.Unknown;
        }

        #region Withdraw

        async Task<ICallResult<SharedId>> IWithdraw.WithdrawAsync(WithdrawRequest request, CancellationToken ct)
            => await WithdrawAsync(request, ct).ConfigureAwait(false);

        public WithdrawOptions WithdrawOptions { get; } = new WithdrawOptions(_exchangeName)
        {
            ParameterRuleOverwrites = [
                RequestParameterRuleOverride<WithdrawRequest>.NotSupported(x => x.AddressTag),
                RequestParameterRuleOverride<WithdrawRequest>.NotSupported(x => x.Network)
                ],
            ExchangeParameterRules = [
                ExchangeParameterRule.Required(
                    "keyName",
                    aliases: ["key"],
                    description: "The name of the withdrawal address as defined in the web UI",
                    exampleValue: "KucoinBitcoinAddress1")
            ]
        };

        public async Task<HttpResult<SharedId>> WithdrawAsync(WithdrawRequest request, CancellationToken ct)
        {
            var validationError = WithdrawOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var keyName = request.GetParamValue<string>(Exchange, "keyName", "key");

            // Get data
            var withdrawal = await _api.Account.WithdrawAsync(
                KrakenExchange.AssetAliases.CommonToExchangeName(request.Asset),
                keyName!,
                request.Quantity,
                request.Address,
                ct: ct).ConfigureAwait(false);
            if (!withdrawal.Success)
                return HttpResult.Fail<SharedId>(withdrawal);

            return HttpResult.Ok(withdrawal, new SharedId(withdrawal.Data.ReferenceId));
        }

        #endregion

    }
}
