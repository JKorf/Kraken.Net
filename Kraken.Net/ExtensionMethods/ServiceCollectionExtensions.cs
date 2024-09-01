using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Interfaces;
using Kraken.Net.Clients;
using Kraken.Net.Interfaces;
using Kraken.Net.Interfaces.Clients;
using Kraken.Net.Objects.Options;
using Kraken.Net.SymbolOrderBooks;
using System;
using System.Net;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for DI
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the IKrakenClient and IKrakenSocketClient to the sevice collection so they can be injected
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="defaultRestOptionsDelegate">Set default options for the rest client</param>
        /// <param name="defaultSocketOptionsDelegate">Set default options for the socket client</param>
        /// <param name="socketClientLifeTime">The lifetime of the IKrakenSocketClient for the service collection. Defaults to Singleton.</param>
        /// <returns></returns>
        public static IServiceCollection AddKraken(
            this IServiceCollection services,
            Action<KrakenRestOptions>? defaultRestOptionsDelegate = null,
            Action<KrakenSocketOptions>? defaultSocketOptionsDelegate = null,
            ServiceLifetime? socketClientLifeTime = null)
        {
            var restOptions = KrakenRestOptions.Default.Copy();

            if (defaultRestOptionsDelegate != null)
            {
                defaultRestOptionsDelegate(restOptions);
                KrakenRestClient.SetDefaultOptions(defaultRestOptionsDelegate);
            }

            if (defaultSocketOptionsDelegate != null)
                KrakenSocketClient.SetDefaultOptions(defaultSocketOptionsDelegate);

            services.AddHttpClient<IKrakenRestClient, KrakenRestClient>(options =>
            {
                options.Timeout = restOptions.RequestTimeout;
            }).ConfigurePrimaryHttpMessageHandler(() => {
                var handler = new HttpClientHandler();
                if (restOptions.Proxy != null)
                {
                    handler.Proxy = new WebProxy
                    {
                        Address = new Uri($"{restOptions.Proxy.Host}:{restOptions.Proxy.Port}"),
                        Credentials = restOptions.Proxy.Password == null ? null : new NetworkCredential(restOptions.Proxy.Login, restOptions.Proxy.Password)
                    };
                }
                return handler;
            });

            services.AddTransient<ICryptoRestClient, CryptoRestClient>();
            services.AddTransient<ICryptoSocketClient, CryptoSocketClient>();
            services.AddTransient<IKrakenOrderBookFactory, KrakenOrderBookFactory>();
            services.AddTransient(x => x.GetRequiredService<IKrakenRestClient>().SpotApi.CommonSpotClient);

            services.RegisterSharedRestInterfaces(x => x.GetRequiredService<IKrakenRestClient>().SpotApi.SharedClient);
            services.RegisterSharedSocketInterfaces(x => x.GetRequiredService<IKrakenSocketClient>().SpotApi.SharedClient);

            if (socketClientLifeTime == null)
                services.AddSingleton<IKrakenSocketClient, KrakenSocketClient>();
            else
                services.Add(new ServiceDescriptor(typeof(IKrakenSocketClient), typeof(KrakenSocketClient), socketClientLifeTime.Value));
            return services;
        }
    }
}
