﻿using Kraken.Net.Converters;
using Kraken.Net.Enums;
using Kraken.Net.Objects.Models;

namespace Kraken.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Kraken exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface IKrakenRestClientSpotApiExchangeData
    {
        /// <summary>
        /// Get the server time
        /// <para><a href="https://docs.kraken.com/api/docs/rest-api/get-server-time" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Server time</returns>
        Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default);

        /// <summary>
        /// Get the system status
        /// <para><a href="https://docs.kraken.com/api/docs/rest-api/get-system-status" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>System status</returns>
        Task<WebCallResult<KrakenSystemStatus>> GetSystemStatusAsync(CancellationToken ct = default);

        /// <summary>
        /// Get a list of assets and info about them
        /// <para><a href="https://docs.kraken.com/api/docs/rest-api/get-asset-info" /></para>
        /// </summary>
        /// <param name="assets">Filter list for specific assets, for example `ETH`</param>
        /// <param name="newAssetNameResponse">When set to true the asset names will be in the new format, for example `BTC` instead of `XBT`. Default is false.</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary of asset info</returns>
        Task<WebCallResult<Dictionary<string, KrakenAssetInfo>>> GetAssetsAsync(IEnumerable<string>? assets = null, bool? newAssetNameResponse = null, CancellationToken ct = default);

        /// <summary>
        /// Get a list of symbols and info about them
        /// <para><a href="https://docs.kraken.com/api/docs/rest-api/get-tradable-asset-pairs" /></para>
        /// </summary>
        /// <param name="countryCode">Filter what's available for a specific country/region</param>
        /// <param name="symbols">Filter list for specific symbols, for example `ETHUSDT`</param>
        /// <param name="newAssetNameResponse">When set to true the asset names will be in the new format, for example `BTC` instead of `XBT`. Default is false.</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary of symbol info</returns>
        Task<WebCallResult<Dictionary<string, KrakenSymbol>>> GetSymbolsAsync(IEnumerable<string>? symbols = null, string? countryCode = null, bool? newAssetNameResponse = null, CancellationToken ct = default);

        /// <summary>
        /// Get tickers for symbol
        /// <para><a href="https://docs.kraken.com/api/docs/rest-api/get-ticker-information" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to get tickers for, for example `ETHUSDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with ticker info</returns>
        Task<WebCallResult<Dictionary<string, KrakenRestTick>>> GetTickerAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get tickers for symbols
        /// <para><a href="https://docs.kraken.com/api/docs/rest-api/get-ticker-information" /></para>
        /// </summary>
        /// <param name="symbols">Symbols to get tickers for, for example `ETHUSDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with ticker info</returns>
        Task<WebCallResult<Dictionary<string, KrakenRestTick>>> GetTickersAsync(IEnumerable<string>? symbols = null, CancellationToken ct = default);

        /// <summary>
        /// Gets kline data for a symbol
        /// <para><a href="https://docs.kraken.com/api/docs/rest-api/get-ohlc-data" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to get data for, for example `ETHUSDT`</param>
        /// <param name="interval">The interval of the klines</param>
        /// <param name="since">Return klines since a specific time</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Kline data</returns>
        Task<WebCallResult<KrakenKlinesResult>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? since = null, CancellationToken ct = default);

        /// <summary>
        /// Get the order book for a symbol
        /// <para><a href="https://docs.kraken.com/api/docs/rest-api/get-order-book" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to get the book for, for example `ETHUSDT`</param>
        /// <param name="limit">Limit to book to the best x bids/asks</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order book for the symbol</returns>
        Task<WebCallResult<KrakenOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get a list of recent trades for a symbol
        /// <para><a href="https://docs.kraken.com/api/docs/rest-api/get-recent-trades" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to get trades for, for example `ETHUSDT`</param>
        /// <param name="since">Return trades since a specific time</param>
        /// <param name="limit">Max amount of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Recent trades</returns>
        Task<WebCallResult<KrakenTradesResult>> GetTradeHistoryAsync(string symbol, DateTime? since = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get spread data for a symbol
        /// <para><a href="https://docs.kraken.com/api/docs/rest-api/get-recent-spreads" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to get spread data for, for example `ETHUSDT`</param>
        /// <param name="since">Return spread data since a specific time</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Spread data</returns>
        Task<WebCallResult<KrakenSpreadsResult>> GetRecentSpreadAsync(string symbol, DateTime? since = null, CancellationToken ct = default);

    }
}
