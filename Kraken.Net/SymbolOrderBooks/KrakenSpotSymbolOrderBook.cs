using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.OrderBook;
using CryptoExchange.Net.Sockets;
using Force.Crc32;
using Kraken.Net.Clients;
using Kraken.Net.Interfaces.Clients;
using Kraken.Net.Objects.Models;
using Kraken.Net.Objects.Options;
using Microsoft.Extensions.Logging;

namespace Kraken.Net.SymbolOrderBooks
{
    /// <summary>
    /// Live order book implementation
    /// </summary>
    public class KrakenSpotSymbolOrderBook : SymbolOrderBook
    {
        private readonly IKrakenSocketClient _socketClient;
        private readonly bool _clientOwner;
        private bool _initialSnapshotDone;
        private readonly TimeSpan _initialDataTimeout;

        /// <summary>
        /// Create a new order book instance
        /// </summary>
        /// <param name="symbol">The symbol the order book is for</param>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public KrakenSpotSymbolOrderBook(string symbol, Action<KrakenOrderBookOptions>? optionsDelegate = null)
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
        public KrakenSpotSymbolOrderBook(string symbol,
            Action<KrakenOrderBookOptions>? optionsDelegate,
            ILogger<KrakenSpotSymbolOrderBook>? logger,
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
            var result = await _socketClient.SpotApi.SubscribeToOrderBookUpdatesAsync(Symbol, Levels!.Value, ProcessUpdate).ConfigureAwait(false);
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

        /// <inheritdoc />
        protected override void DoReset()
        {
            _initialSnapshotDone = false;
        }

        private void ProcessUpdate(DataEvent<KrakenStreamOrderBook> data)
        {
            if (!_initialSnapshotDone)
            {
                var maxNumber = Math.Max(data.Data.Bids.Max(b => b.Sequence), data.Data.Asks.Max(b => b.Sequence));
                SetInitialOrderBook(maxNumber, data.Data.Bids, data.Data.Asks);
                _initialSnapshotDone = true;
            }
            else
            {
                UpdateOrderBook(data.Data.Bids, data.Data.Asks);
                AddChecksum((int)data.Data.Checksum!);
            }
        }

        /// <inheritdoc />
        protected override bool DoChecksum(int checksum)
        {
            var checksumValues = new List<string>();
            for (var i = 0; i < 10; i++)
            {
                var ask = (KrakenStreamOrderBookEntry)_asks.ElementAt(i).Value;
                checksumValues.Add(ToChecksumString(ask.RawPrice));
                checksumValues.Add(ToChecksumString(ask.RawQuantity));
            }
            for (var i = 0; i < 10; i++)
            {
                var bid = (KrakenStreamOrderBookEntry)_bids.ElementAt(i).Value;
                checksumValues.Add(ToChecksumString(bid.RawPrice));
                checksumValues.Add(ToChecksumString(bid.RawQuantity));
            }

            var checksumString = string.Join("", checksumValues);
            var ourChecksumUtf = (int)Crc32Algorithm.Compute(Encoding.UTF8.GetBytes(checksumString));

            if (ourChecksumUtf != checksum)
            {
                _logger.Log(LogLevel.Warning, $"Invalid checksum. Received from server: {checksum}, calculated local: {ourChecksumUtf}");
                return false;
            }

            return true;
        }

        private static string ToChecksumString(string value)
        {
            return value.Replace(".", "").TrimStart('0');
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
