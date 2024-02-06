using System;
using System.Text.RegularExpressions;

namespace Kraken.Net.ExtensionMethods
{
    /// <summary>
    /// Extension methods specific to using the Kraken API
    /// </summary>
    public static class KrakenExtensionMethods
    {
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
