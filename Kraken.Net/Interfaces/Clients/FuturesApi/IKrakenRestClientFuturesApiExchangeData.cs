using Kraken.Net.Enums;
using Kraken.Net.Objects.Models.Futures;

namespace Kraken.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// Kraken futures exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface IKrakenRestClientFuturesApiExchangeData
    {
        /// <summary>
        /// Get fee schedules
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/trading/get-fee-schedules-v-3" /><br />
        /// Endpoint:<br />
        /// GET /derivatives/api/v3/feeschedules
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFeeSchedule[]>> GetFeeSchedulesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get historical funding rates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/trading/historical-funding-rates" /><br />
        /// Endpoint:<br />
        /// GET /derivatives/api/v4/historicalfundingrates
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `PF_ETHUSD`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFundingRate[]>> GetHistoricalFundingRatesAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get klines/candle data
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/charts/candles" /><br />
        /// Endpoint:<br />
        /// GET /api/charts/v1/{tickType}/{symbol}/{interval}
        /// </para>
        /// </summary>
        /// <param name="tickType">Type of price tick</param>
        /// <param name="symbol">The symbol, for example `PF_ETHUSD`</param>
        /// <param name="interval">Interval of the klines</param>
        /// <param name="startTime">Start time</param>
        /// <param name="endTime">End time</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesKlines>> GetKlinesAsync(TickType tickType, string symbol, FuturesKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get the orderbook
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/trading/get-orderbook" /><br />
        /// Endpoint:<br />
        /// GET /derivatives/api/v3/orderbook
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `PF_ETHUSD`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesOrderBook>> GetOrderBookAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get platform notifications
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/trading/get-notifications" /><br />
        /// Endpoint:<br />
        /// GET /derivatives/api/v3/notifications
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesPlatfromNotificationResult>> GetPlatformNotificationsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get a list of symbols
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/trading/get-instruments" /><br />
        /// Endpoint:<br />
        /// GET /derivatives/api/v3/instruments
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesSymbol[]>> GetSymbolsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get a list of symbols statuses
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/trading/instruments-status" /><br />
        /// Endpoint:<br />
        /// GET /derivatives/api/v3/instruments/status
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesSymbolStatus[]>> GetSymbolStatusAsync(CancellationToken ct = default);

        /// <summary>
        /// Get ticker
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/trading/get-ticker" /><br />
        /// Endpoint:<br />
        /// GET /derivatives/api/v3/tickers/{symbol}
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol to get ticker for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesTicker>> GetTickerAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get tickers
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/trading/get-tickers" /><br />
        /// Endpoint:<br />
        /// GET /derivatives/api/v3/tickers
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesTicker[]>> GetTickersAsync(CancellationToken ct = default);

        /// <summary>
        /// Get list of recent trades
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/trading/get-history" /><br />
        /// Endpoint:<br />
        /// GET /derivatives/api/v3/history
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `PF_ETHUSD`</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesTrade[]>> GetTradesAsync(string symbol, DateTime? startTime = null, CancellationToken ct = default);
    }
}
