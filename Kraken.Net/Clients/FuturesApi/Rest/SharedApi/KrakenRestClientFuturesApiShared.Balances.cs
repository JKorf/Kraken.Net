using CryptoExchange.Net.SharedApis;

namespace Kraken.Net.Clients.FuturesApi
{
	internal partial class KrakenRestClientFuturesSharedApi
	{

        #region Get Balances

        async Task<IExchangeCallResult<SharedBalance[]>> IGetBalances.GetBalancesAsync(GetBalancesRequest request, CancellationToken ct)
            => await GetBalancesAsync(request, ct).ConfigureAwait(false);

		public GetBalancesOptions GetBalancesOptions { get; } = new GetBalancesOptions(_exchangeName, AccountTypeFilter.Futures);

		public async Task<HttpResult<SharedBalance[]>> GetBalancesAsync(GetBalancesRequest request, CancellationToken ct)
		{
			var validationError = GetBalancesOptions.ValidateRequest(request, this);
			if (validationError != null)
				return HttpResult.Fail<SharedBalance[]>(Exchange, validationError);

			var result = await _api.Account.GetBalancesAsync(ct: ct).ConfigureAwait(false);
			if (!result.Success)
				return HttpResult.Fail<SharedBalance[]>(result);

			var balances = result.Data.MultiCollateralMarginAccount.Currencies.Select(x =>
				new SharedBalance(
					SupportedTradingModes,
					x.Key,
					x.Value.Available,
					x.Value.Quantity)).ToList();
			foreach (var balance in result.Data.MarginAccounts)
			{
				foreach (var currency in balance.Balances)
				{
					balances.Add(new SharedBalance(
						SupportedTradingModes,
						currency.Key,
						currency.Value,
						currency.Value)
					{ IsolatedMarginSymbol = balance.Symbol });
				}
			}

			return HttpResult.Ok(result, balances.ToArray());
		}

        #endregion

	}
}
