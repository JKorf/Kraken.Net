using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.SharedApis;
using Kraken.Net;
using Kraken.Net.Clients;
using Kraken.Net.Interfaces;
using Kraken.Net.Interfaces.Clients;
using Kraken.Net.Interfaces.Clients.FuturesApi;
using Kraken.Net.Interfaces.Clients.SpotApi;
using Kraken.Net.Objects.Options;
using Kraken.Net.SymbolOrderBooks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for DI
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Add services such as the IKrakenRestClient and IKrakenSocketClient. Configures the services based on the provided configuration.<br />
        /// See <see href="https://github.com/JKorf/HTX.Net/blob/master/Examples/example-config.json" /> for an example of how to set up the configuration.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The configuration(section) containing the options</param>
        /// <returns></returns>
        public static IServiceCollection AddKraken(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var options = KrakenOptions.CreateFromConfiguration(configuration);
            services.AddSingleton(Options.Options.Create(options.Rest));
            services.AddSingleton(Options.Options.Create(options.Socket));
            services.AddSingleton(Options.Options.Create(options));

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
            var options = KrakenOptions.Create(optionsDelegate);
            services.AddSingleton(Options.Options.Create(options.Rest));
            services.AddSingleton(Options.Options.Create(options.Socket));
            services.AddSingleton(Options.Options.Create(options));

            return AddKrakenCore(services, options.SocketClientLifeTime);
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
                var options = serviceProvider.GetRequiredService<IOptions<KrakenRestOptions>>().Value;
                return LibraryHelpers.CreateHttpClientMessageHandler(options);
            }).SetHandlerLifetime(Timeout.InfiniteTimeSpan);
            services.Add(new ServiceDescriptor(typeof(IKrakenSocketClient), x => { return new KrakenSocketClient(x.GetRequiredService<IOptions<KrakenSocketOptions>>(), x.GetRequiredService<ILoggerFactory>()); }, socketClientLifeTime ?? ServiceLifetime.Singleton));

            services.AddTransient<IKrakenOrderBookFactory, KrakenOrderBookFactory>();
            services.AddTransient<IKrakenTrackerFactory, KrakenTrackerFactory>();
            services.AddTransient<ITrackerFactory, KrakenTrackerFactory>();
            services.AddSingleton<IKrakenUserClientProvider, KrakenUserClientProvider>(x =>
            new KrakenUserClientProvider(
                x.GetRequiredService<IHttpClientFactory>().CreateClient(typeof(IKrakenRestClient).Name),
                x.GetRequiredService<ILoggerFactory>(),
                x.GetRequiredService<IOptions<KrakenRestOptions>>(),
                x.GetRequiredService<IOptions<KrakenSocketOptions>>()));


            services.RegisterSharedRestInterfaces(x => x.GetRequiredService<IKrakenRestClient>().SpotApi.SharedClient);
            services.RegisterSharedSocketInterfaces(x => x.GetRequiredService<IKrakenSocketClient>().SpotApi.SharedClient);
            services.RegisterSharedRestInterfaces(x => x.GetRequiredService<IKrakenRestClient>().FuturesApi.SharedClient);
            services.RegisterSharedSocketInterfaces(x => x.GetRequiredService<IKrakenSocketClient>().FuturesApi.SharedClient);

            services.RegisterSharedApiClient<
                IKrakenSharedApiClient,
                KrakenSharedApiClient>(sharedApis => sharedApis
                    .Add(client => client.SpotRest)
                    .Add(client => client.FuturesRest)
                    .Add(client => client.SpotSocket)
                    .Add(client => client.FuturesSocket));

            return services;
        }
    }
}
