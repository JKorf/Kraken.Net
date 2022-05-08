using Kraken.Net.Clients;
using Kraken.Net.Interfaces.Clients;
using Kraken.Net.Objects;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.RegularExpressions;

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
        /// <param name="defaultOptionsCallback">Set default options for the client</param>
        /// <param name="socketClientLifeTime">The lifetime of the IKrakenSocketClient for the service collection. Defaults to Scoped.</param>
        /// <returns></returns>
        public static IServiceCollection AddKraken(
            this IServiceCollection services, 
            Action<KrakenClientOptions, KrakenSocketClientOptions>? defaultOptionsCallback = null,
            ServiceLifetime? socketClientLifeTime = null)
        {
            if (defaultOptionsCallback != null)
            {
                var options = new KrakenClientOptions();
                var socketOptions = new KrakenSocketClientOptions();
                defaultOptionsCallback?.Invoke(options, socketOptions);

                KrakenClient.SetDefaultOptions(options);
                KrakenSocketClient.SetDefaultOptions(socketOptions);
            }

            services.AddTransient<IKrakenClient, KrakenClient>();
            if (socketClientLifeTime == null)
                services.AddScoped<IKrakenSocketClient, KrakenSocketClient>();
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
