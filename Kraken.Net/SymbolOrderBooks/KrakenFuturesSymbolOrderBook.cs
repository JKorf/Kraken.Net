using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.OrderBook;
using Kraken.Net.Clients;
using Kraken.Net.Interfaces.Clients;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Options;
using Microsoft.Extensions.Logging;

namespace Kraken.Net.SymbolOrderBooks
{
    /// <summary>
    /// Live order book implementation
    /// </summary>
    public class KrakenFuturesSymbolOrderBook : SymbolOrderBook
    {
        private readonly IKrakenSocketClient _socketClient;
        private readonly bool _clientOwner;
        private readonly TimeSpan _initialDataTimeout;

        /// <summary>
        /// Create a new order book instance
        /// </summary>
        /// <param name="symbol">The symbol the order book is for</param>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public KrakenFuturesSymbolOrderBook(string symbol, Action<KrakenOrderBookOptions>? optionsDelegate = null)
            : this(symbol, optionsDelegate, null, null)
        {
        }

        /// <summary>
        /// Create a new order book instance
        /// </summary>
        /// <param name="symbol">The symbol the order book is for</param>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        /// <param name="logger">Logger</param>
        /// <param name="socketClient">Socket client instance</param>
        public KrakenFuturesSymbolOrderBook(string symbol,
            Action<KrakenOrderBookOptions>? optionsDelegate,
            ILogger<KrakenFuturesSymbolOrderBook>? logger,
            IKrakenSocketClient? socketClient) : base(logger, "Kraken", symbol)
        {
            var options = KrakenOrderBookOptions.Default.Copy();
            if (optionsDelegate != null)
                optionsDelegate(options);
            Initialize(options);

            _sequencesAreConsecutive = false;
            _strictLevels = true;
            _initialDataTimeout = options?.InitialDataTimeout ?? TimeSpan.FromSeconds(30);

            _socketClient = socketClient ?? new KrakenSocketClient();
            _clientOwner = socketClient == null;

            Levels = options?.Limit ?? 10;
        }

        /// <inheritdoc />
        protected override async Task<CallResult<UpdateSubscription>> DoStartAsync(CancellationToken ct)
        {
            var result = await _socketClient.FuturesApi.SubscribeToOrderBookUpdatesAsync(new[] { Symbol }, ProcessSnapshot, ProcessUpdate).ConfigureAwait(false);
            if (!result)
                return result;

            if (ct.IsCancellationRequested)
            {
                await result.Data.CloseAsync().ConfigureAwait(false);
                return result.AsError<UpdateSubscription>(new CancellationRequestedError());
            }

            Status = OrderBookStatus.Syncing;

            var setResult = await WaitForSetOrderBookAsync(_initialDataTimeout, ct).ConfigureAwait(false);
            return setResult ? result : new CallResult<UpdateSubscription>(setResult.Error!);
        }

        private void ProcessSnapshot(DataEvent<KrakenFuturesBookSnapshotUpdate> data)
        {
            SetInitialOrderBook(data.Data.Sequence, data.Data.Bids, data.Data.Asks);
        }

        private void ProcessUpdate(DataEvent<KrakenFuturesBookUpdate> data)
        {
            if (data.Data.Side == Enums.OrderSide.Sell)
                UpdateOrderBook(data.Data.Sequence, Array.Empty<ISymbolOrderBookEntry>(), new[] { data.Data });
            else
                UpdateOrderBook(data.Data.Sequence, new[] { data.Data }, Array.Empty<ISymbolOrderBookEntry>());
        }

        /// <inheritdoc />
        protected override async Task<CallResult<bool>> DoResyncAsync(CancellationToken ct)
        {
            return await WaitForSetOrderBookAsync(_initialDataTimeout, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (_clientOwner)
                _socketClient?.Dispose();

            base.Dispose(disposing);
        }
    }
}
