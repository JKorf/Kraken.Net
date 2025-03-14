using CryptoExchange.Net.Clients;
using Kraken.Net;
using Kraken.Net.Clients;
using Kraken.Net.Interfaces;
using Kraken.Net.Interfaces.Clients;
using Kraken.Net.Objects.Options;
using Kraken.Net.SymbolOrderBooks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for DI
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Add services such as the IKrakenRestClient and IKrakenSocketClient. Configures the services based on the provided configuration.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The configuration(section) containing the options</param>
        /// <returns></returns>
        public static IServiceCollection AddKraken(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var options = new KrakenOptions();
            // Reset environment so we know if theyre overriden
            options.Rest.Environment = null!;
            options.Socket.Environment = null!;
            configuration.Bind(options);

            if (options.Rest == null || options.Socket == null)
                throw new ArgumentException("Options null");

            var restEnvName = options.Rest.Environment?.Name ?? options.Environment?.Name ?? KrakenEnvironment.Live.Name;
            var socketEnvName = options.Socket.Environment?.Name ?? options.Environment?.Name ?? KrakenEnvironment.Live.Name;
            options.Rest.Environment = KrakenEnvironment.GetEnvironmentByName(restEnvName) ?? options.Rest.Environment!;
            options.Rest.ApiCredentials = options.Rest.ApiCredentials ?? options.ApiCredentials;
            options.Socket.Environment = KrakenEnvironment.GetEnvironmentByName(socketEnvName) ?? options.Socket.Environment!;
            options.Socket.ApiCredentials = options.Socket.ApiCredentials ?? options.ApiCredentials;


            services.AddSingleton(x => Options.Options.Create(options.Rest));
            services.AddSingleton(x => Options.Options.Create(options.Socket));

            return AddKrakenCore(services, options.SocketClientLifeTime);
        }

        /// <summary>
        /// Add services such as the IKrakenRestClient and IKrakenSocketClient. Services will be configured based on the provided options.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsDelegate">Set options for the Kraken services</param>
        /// <returns></returns>
        public static IServiceCollection AddKraken(
            this IServiceCollection services,
            Action<KrakenOptions>? optionsDelegate = null)
        {
            var options = new KrakenOptions();
            // Reset environment so we know if theyre overriden
            options.Rest.Environment = null!;
            options.Socket.Environment = null!;
            optionsDelegate?.Invoke(options);
            if (options.Rest == null || options.Socket == null)
                throw new ArgumentException("Options null");

            options.Rest.Environment = options.Rest.Environment ?? options.Environment ?? KrakenEnvironment.Live;
            options.Rest.ApiCredentials = options.Rest.ApiCredentials ?? options.ApiCredentials;
            options.Socket.Environment = options.Socket.Environment ?? options.Environment ?? KrakenEnvironment.Live;
            options.Socket.ApiCredentials = options.Socket.ApiCredentials ?? options.ApiCredentials;

            services.AddSingleton(x => Options.Options.Create(options.Rest));
            services.AddSingleton(x => Options.Options.Create(options.Socket));

            return AddKrakenCore(services, options.SocketClientLifeTime);
        }

        /// <summary>
        /// DEPRECATED; use <see cref="AddKraken(IServiceCollection, Action{KrakenOptions}?)" /> instead
        /// </summary>
        public static IServiceCollection AddKraken(
            this IServiceCollection services,
            Action<KrakenRestOptions> restDelegate,
            Action<KrakenSocketOptions>? socketDelegate = null,
            ServiceLifetime? socketClientLifeTime = null)
        {
            services.Configure<KrakenRestOptions>((x) => { restDelegate?.Invoke(x); });
            services.Configure<KrakenSocketOptions>((x) => { socketDelegate?.Invoke(x); });

            return AddKrakenCore(services, socketClientLifeTime);
        }

        private static IServiceCollection AddKrakenCore(
            this IServiceCollection services,
            ServiceLifetime? socketClientLifeTime = null)
        {
            services.AddHttpClient<IKrakenRestClient, KrakenRestClient>((client, serviceProvider) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<KrakenRestOptions>>().Value;
                client.Timeout = options.RequestTimeout;
                return new KrakenRestClient(client, serviceProvider.GetRequiredService<ILoggerFactory>(), serviceProvider.GetRequiredService<IOptions<KrakenRestOptions>>());
            }).ConfigurePrimaryHttpMessageHandler((serviceProvider) => {
                var handler = new HttpClientHandler();
                try
                {
                    handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                    handler.DefaultProxyCredentials = CredentialCache.DefaultCredentials;
                }
                catch (PlatformNotSupportedException) { }
                catch (NotImplementedException) { } // Mono runtime throws NotImplementedException for DefaultProxyCredentials setting

                var options = serviceProvider.GetRequiredService<IOptions<KrakenRestOptions>>().Value;
                if (options.Proxy != null)
                {
                    handler.Proxy = new WebProxy
                    {
                        Address = new Uri($"{options.Proxy.Host}:{options.Proxy.Port}"),
                        Credentials = options.Proxy.Password == null ? null : new NetworkCredential(options.Proxy.Login, options.Proxy.Password)
                    };
                }
                return handler;
            });
            services.Add(new ServiceDescriptor(typeof(IKrakenSocketClient), x => { return new KrakenSocketClient(x.GetRequiredService<IOptions<KrakenSocketOptions>>(), x.GetRequiredService<ILoggerFactory>()); }, socketClientLifeTime ?? ServiceLifetime.Singleton));

            services.AddTransient<ICryptoRestClient, CryptoRestClient>();
            services.AddTransient<ICryptoSocketClient, CryptoSocketClient>();
            services.AddTransient<IKrakenOrderBookFactory, KrakenOrderBookFactory>();
            services.AddTransient<IKrakenTrackerFactory, KrakenTrackerFactory>();

            services.RegisterSharedRestInterfaces(x => x.GetRequiredService<IKrakenRestClient>().SpotApi.SharedClient);
            services.RegisterSharedSocketInterfaces(x => x.GetRequiredService<IKrakenSocketClient>().SpotApi.SharedClient);
            services.RegisterSharedRestInterfaces(x => x.GetRequiredService<IKrakenRestClient>().FuturesApi.SharedClient);
            services.RegisterSharedSocketInterfaces(x => x.GetRequiredService<IKrakenSocketClient>().FuturesApi.SharedClient);

            return services;
        }
    }
}
