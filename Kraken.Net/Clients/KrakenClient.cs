using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using Kraken.Net.Clients.SpotApi;
using Kraken.Net.Interfaces.Clients;
using Kraken.Net.Interfaces.Clients.SpotApi;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Internal;

namespace Kraken.Net.Clients
{
    /// <inheritdoc cref="IKrakenClient" />
    public class KrakenClient : BaseRestClient, IKrakenClient
    {
        #region fields
        #endregion

        #region Api clients
        /// <inheritdoc />
        public IKrakenClientSpotApi SpotApi { get; }
        #endregion

        #region ctor
        /// <summary>
        /// Create a new instance of KrakenClient using the default options
        /// </summary>
        public KrakenClient() : this(KrakenClientOptions.Default)
        {
        }

        /// <summary>
        /// Create a new instance of KrakenClient using provided options
        /// </summary>
        /// <param name="options">The options to use for this client</param>
        public KrakenClient(KrakenClientOptions options) : base("Kraken", options)
        {
            SpotApi = AddApiClient(new KrakenClientSpotApi(log, this, options));
        }
        #endregion

        #region methods

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="options">Options to use as default</param>
        public static void SetDefaultOptions(KrakenClientOptions options)
        {
            KrakenClientOptions.Default = options;
        }

        #endregion

        internal async Task<WebCallResult<T>> Execute<T>(RestApiClient apiClient, Uri url, HttpMethod method, CancellationToken ct, Dictionary<string, object>? parameters = null, bool signed = false, int weight = 1, bool ignoreRatelimit = false)
        {
            var result = await SendRequestAsync<KrakenResult<T>>(apiClient, url, method, ct, parameters, signed, requestWeight: weight, ignoreRatelimit: ignoreRatelimit).ConfigureAwait(false);
            if (!result)
                return result.AsError<T>(result.Error!);

            if (result.Data.Error.Any())
                return result.AsError<T>(new ServerError(string.Join(", ", result.Data.Error)));

            return result.As(result.Data.Result);
        }
    }
}
