using Kraken.Net.Interfaces.Clients;
using Kraken.Net.Objects.Options;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace Kraken.Net.Clients
{
    /// <inheritdoc />
    public class KrakenUserClientProvider : IKrakenUserClientProvider
    {
        private ConcurrentDictionary<string, IKrakenRestClient> _restClients = new ConcurrentDictionary<string, IKrakenRestClient>();
        private ConcurrentDictionary<string, IKrakenSocketClient> _socketClients = new ConcurrentDictionary<string, IKrakenSocketClient>();

        private readonly IOptions<KrakenRestOptions> _restOptions;
        private readonly IOptions<KrakenSocketOptions> _socketOptions;
        private readonly HttpClient _httpClient;
        private readonly ILoggerFactory? _loggerFactory;

        /// <inheritdoc />
        public string ExchangeName => KrakenExchange.ExchangeName;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="optionsDelegate">Options to use for created clients</param>
        public KrakenUserClientProvider(Action<KrakenOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate).Rest), Options.Create(ApplyOptionsDelegate(optionsDelegate).Socket))
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        public KrakenUserClientProvider(
            HttpClient? httpClient,
            ILoggerFactory? loggerFactory,
            IOptions<KrakenRestOptions> restOptions,
            IOptions<KrakenSocketOptions> socketOptions)
        {
            _httpClient = httpClient ?? new HttpClient();
            _httpClient.Timeout = restOptions.Value.RequestTimeout;
            _loggerFactory = loggerFactory;
            _restOptions = restOptions;
            _socketOptions = socketOptions;
        }

        /// <inheritdoc />
        public void InitializeUserClient(string userIdentifier, ApiCredentials credentials, KrakenEnvironment? environment = null)
        {
            CreateRestClient(userIdentifier, credentials, environment);
            CreateSocketClient(userIdentifier, credentials, environment);
        }

        /// <inheritdoc />
        public void ClearUserClients(string userIdentifier)
        {
            _restClients.TryRemove(userIdentifier, out _);
            _socketClients.TryRemove(userIdentifier, out _);
        }

        /// <inheritdoc />
        public IKrakenRestClient GetRestClient(string userIdentifier, ApiCredentials? credentials = null, KrakenEnvironment? environment = null)
        {
            if (!_restClients.TryGetValue(userIdentifier, out var client) || client.Disposed)
                client = CreateRestClient(userIdentifier, credentials, environment);

            return client;
        }

        /// <inheritdoc />
        public IKrakenSocketClient GetSocketClient(string userIdentifier, ApiCredentials? credentials = null, KrakenEnvironment? environment = null)
        {
            if (!_socketClients.TryGetValue(userIdentifier, out var client) || client.Disposed)
                client = CreateSocketClient(userIdentifier, credentials, environment);

            return client;
        }

        private IKrakenRestClient CreateRestClient(string userIdentifier, ApiCredentials? credentials, KrakenEnvironment? environment)
        {
            var clientRestOptions = SetRestEnvironment(environment);
            var client = new KrakenRestClient(_httpClient, _loggerFactory, clientRestOptions);
            if (credentials != null)
            {
                client.SetApiCredentials(credentials);
                _restClients.TryAdd(userIdentifier, client);
            }
            return client;
        }

        private IKrakenSocketClient CreateSocketClient(string userIdentifier, ApiCredentials? credentials, KrakenEnvironment? environment)
        {
            var clientSocketOptions = SetSocketEnvironment(environment);
            var client = new KrakenSocketClient(clientSocketOptions!, _loggerFactory);
            if (credentials != null)
            {
                client.SetApiCredentials(credentials);
                _socketClients.TryAdd(userIdentifier, client);
            }
            return client;
        }

        private IOptions<KrakenRestOptions> SetRestEnvironment(KrakenEnvironment? environment)
        {
            if (environment == null)
                return _restOptions;

            var newRestClientOptions = new KrakenRestOptions();
            var restOptions = _restOptions.Value.Set(newRestClientOptions);
            newRestClientOptions.Environment = environment;
            return Options.Create(newRestClientOptions);
        }

        private IOptions<KrakenSocketOptions> SetSocketEnvironment(KrakenEnvironment? environment)
        {
            if (environment == null)
                return _socketOptions;

            var newSocketClientOptions = new KrakenSocketOptions();
            var restOptions = _socketOptions.Value.Set(newSocketClientOptions);
            newSocketClientOptions.Environment = environment;
            return Options.Create(newSocketClientOptions);
        }

        private static T ApplyOptionsDelegate<T>(Action<T>? del) where T : new()
        {
            var opts = new T();
            del?.Invoke(opts);
            return opts;
        }
    }
}
