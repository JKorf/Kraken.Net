using System;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.OrderBook;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Interfaces;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Socket;

namespace Kraken.Net
{
    /// <summary>
    /// Live order book implementation
    /// </summary>
    public class KrakenSymbolOrderBook : SymbolOrderBook
    {
        private readonly IKrakenSocketClient socketClient;
        private bool initialSnapshotDone;
        private readonly int limit;

        /// <summary>
        /// Create a new order book instance
        /// </summary>
        /// <param name="symbol">The symbol the order book is for</param>
        /// <param name="limit">The initial limit of entries in the order book</param>
        /// <param name="options">Options for the order book</param>
        public KrakenSymbolOrderBook(string symbol, int limit, KrakenOrderBookOptions? options = null) : base(symbol, options ?? new KrakenOrderBookOptions())
        {
            socketClient = options?.SocketClient ?? new KrakenSocketClient();

            this.limit = limit;
        }

        /// <inheritdoc />
        protected override async Task<CallResult<UpdateSubscription>> DoStart()
        {
            var result = await socketClient.SubscribeToDepthUpdatesAsync(Symbol, limit, ProcessUpdate).ConfigureAwait(false);
            if (!result)
                return result;

            Status = OrderBookStatus.Syncing;

            var setResult = await WaitForSetOrderBook(10000).ConfigureAwait(false);
            return setResult ? result : new CallResult<UpdateSubscription>(null, setResult.Error);
        }

        /// <inheritdoc />
        protected override void DoReset()
        {
            initialSnapshotDone = false;
        }

        private void ProcessUpdate(KrakenSocketEvent<KrakenStreamOrderBook> data)
        {
            if (!initialSnapshotDone)
            {
                SetInitialOrderBook(DateTime.UtcNow.Ticks, data.Data.Bids, data.Data.Asks);
                initialSnapshotDone = true;
            }
            else
            {
                UpdateOrderBook(DateTime.UtcNow.Ticks, data.Data.Bids, data.Data.Asks);
            }
        }

        /// <inheritdoc />
        protected override async Task<CallResult<bool>> DoResync()
        {
            return await WaitForSetOrderBook(10000).ConfigureAwait(false);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            processBuffer.Clear();
            asks.Clear();
            bids.Clear();

            socketClient?.Dispose();
        }
    }
}
