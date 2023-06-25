using Kraken.Net.Clients;
using Kraken.Net.Interfaces.Clients;
using Kraken.Net.Objects;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Net;
using System.Text.RegularExpressions;
using Kraken.Net.Objects.Options;
using Kraken.Net.Interfaces;
using Kraken.Net.SymbolOrderBooks;

namespace Kraken.Net
{
    /// <summary>
    /// Helper methods for Kraken
    /// </summary>
    public static class KrakenHelpers
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

            services.AddSingleton<IKrakenOrderBookFactory, KrakenOrderBookFactory>();
            services.AddTransient<IKrakenRestClient, KrakenRestClient>();
            if (socketClientLifeTime == null)
                services.AddSingleton<IKrakenSocketClient, KrakenSocketClient>();
            else
                services.Add(new ServiceDescriptor(typeof(IKrakenSocketClient), typeof(KrakenSocketClient), socketClientLifeTime.Value));
            return services;
        }

        /// <summary>
        /// Validate the string is a valid Kraken symbol.
        /// </summary>
        /// <param name="symbolString">string to validate</param>
        public static string ValidateKrakenSymbol(this string symbolString)
        {
            if (string.IsNullOrEmpty(symbolString))
                throw new ArgumentException("Symbol is not provided");
            if (!Regex.IsMatch(symbolString, "^(([a-z]|[A-Z]|[0-9]|\\.){4,})$"))
                throw new ArgumentException($"{symbolString} is not a valid Kraken symbol. Should be [BaseAsset][QuoteAsset], e.g. ETHXBT");
            return symbolString;
        }

        /// <summary>
        /// Validate the string is a valid Kraken websocket symbol.
        /// </summary>
        /// <param name="symbolString">string to validate</param>
        public static void ValidateKrakenWebsocketSymbol(this string symbolString)
        {
            if (string.IsNullOrEmpty(symbolString))
                throw new ArgumentException("Symbol is not provided");
            if (!Regex.IsMatch(symbolString, "^(([A-Z]|[0-9]|[.]){1,})[/](([A-Z]|[0-9]){1,})$"))
                throw new ArgumentException($"{symbolString} is not a valid Kraken websocket symbol. Should be [BaseAsset]/[QuoteAsset] in ISO 4217-A3 standardized names, e.g. ETH/XBT" +
                                            "Websocket names for pairs are returned in the GetSymbols method in the WebsocketName property.");
        }
    }
}
