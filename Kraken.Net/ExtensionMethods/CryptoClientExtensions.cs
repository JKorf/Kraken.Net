using CryptoExchange.Net.Interfaces.Clients;
using Kraken.Net.Clients;
using Kraken.Net.Interfaces.Clients;

namespace CryptoExchange.Net.Interfaces
{
    /// <summary>
    /// Extensions for the ICryptoRestClient and ICryptoSocketClient interfaces
    /// </summary>
    public static class CryptoClientExtensions
    {
        /// <summary>
        /// Get the Kraken REST Api client
        /// </summary>
        /// <param name="baseClient"></param>
        /// <returns></returns>
        public static IKrakenRestClient Kraken(this ICryptoRestClient baseClient) => baseClient.TryGet<IKrakenRestClient>(() => new KrakenRestClient());

        /// <summary>
        /// Get the Kraken Websocket Api client
        /// </summary>
        /// <param name="baseClient"></param>
        /// <returns></returns>
        public static IKrakenSocketClient Kraken(this ICryptoSocketClient baseClient) => baseClient.TryGet<IKrakenSocketClient>(() => new KrakenSocketClient());
    }
}
