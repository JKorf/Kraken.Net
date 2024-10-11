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
        /// <para><a href="https://docs.futures.kraken.com/#http-api-trading-v3-api-fee-schedules-get-fee-schedules" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<KrakenFeeSchedule>>> GetFeeSchedulesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get historical funding rates
        /// <para><a href="https://docs.futures.kraken.com/#http-api-trading-v3-api-historical-funding-rates-historical-funding-rates" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `PF_ETHUSD`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<KrakenFundingRate>>> GetHistoricalFundingRatesAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get klines/candle data
        /// <para><a href="https://docs.futures.kraken.com/#http-api-charts-candles-market-candles" /></para>
        /// </summary>
        /// <param name="tickType">Type of price tick</param>
        /// <param name="symbol">The symbol, for example `PF_ETHUSD`</param>
        /// <param name="interval">Interval of the klines</param>
        /// <param name="startTime">Start time</param>
        /// <param name="endTime">End time</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesKlines>> GetKlinesAsync(TickType tickType, string symbol, FuturesKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);

        /// <summary>
        /// Get the orderbook
        /// <para><a href="https://docs.futures.kraken.com/#http-api-trading-v3-api-market-data-get-orderbook" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `PF_ETHUSD`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesOrderBook>> GetOrderBookAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get platform notifications
        /// <para><a href="https://docs.futures.kraken.com/#http-api-trading-v3-api-general-get-notifications" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesPlatfromNotificationResult>> GetPlatformNotificationsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get a list of symbols
        /// <para><a href="https://docs.futures.kraken.com/#http-api-trading-v3-api-instrument-details-get-instruments" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<KrakenFuturesSymbol>>> GetSymbolsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get a list of symbols statuses
        /// <para><a href="https://docs.futures.kraken.com/#http-api-trading-v3-api-instrument-details-get-instrument-status-list" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<KrakenFuturesSymbolStatus>>> GetSymbolStatusAsync(CancellationToken ct = default);

        /// <summary>
        /// Get tickers
        /// <para><a href="https://docs.futures.kraken.com/#http-api-trading-v3-api-market-data-get-tickers" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<KrakenFuturesTicker>>> GetTickersAsync(CancellationToken ct = default);

        /// <summary>
        /// Get list of recent trades
        /// <para><a href="https://docs.futures.kraken.com/#http-api-trading-v3-api-market-data-get-trade-history" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `PF_ETHUSD`</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<KrakenFuturesTrade>>> GetTradesAsync(string symbol, DateTime? startTime = null, CancellationToken ct = default);
    }
}