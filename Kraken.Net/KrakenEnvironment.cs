using CryptoExchange.Net.Objects;
using Kraken.Net.Objects;

namespace Kraken.Net
{
    /// <summary>
    /// Kraken environments
    /// </summary>
    public class KrakenEnvironment : TradeEnvironment
    {
        /// <summary>
         /// Spot Rest client address
         /// </summary>
        public string SpotRestBaseAddress { get; }

        /// <summary>
        /// Base address for socket API
        /// </summary>
        public string SpotSocketPublicAddress { get; }

        /// <summary>
        /// Socket base address for the private API
        /// </summary>
        public string SpotSocketPrivateAddress { get; }

        internal KrakenEnvironment(string name,
            string spotRestBaseAddress,
            string spotSocketPublicAddress,
            string spotSocketPrivateAddress) : base(name)
        {
            SpotRestBaseAddress = spotRestBaseAddress;
            SpotSocketPublicAddress = spotSocketPublicAddress;
            SpotSocketPrivateAddress = spotSocketPrivateAddress;
        }

        /// <summary>
        /// Live environment
        /// </summary>
        public static KrakenEnvironment Live { get; }
            = new KrakenEnvironment(TradeEnvironmentNames.Live,
                                   KrakenApiAddresses.Default.SpotRestClientAddress,
                                   KrakenApiAddresses.Default.SpotSocketPublicAddress,
                                   KrakenApiAddresses.Default.SpotSocketPrivateAddress);

        /// <summary>
        /// Create a custom environment
        /// </summary>
        /// <param name="name"></param>
        /// <param name="spotRestAddress"></param>
        /// <param name="spotSocketPublicAddress"></param>
        /// <param name="spotSocketPrivateAddress"></param>
        /// <returns></returns>
        public static KrakenEnvironment CreateCustom(
                        string name,
                        string spotRestAddress,
                        string spotSocketPublicAddress,
                        string spotSocketPrivateAddress)
            => new KrakenEnvironment(name, spotRestAddress, spotSocketPublicAddress, spotSocketPrivateAddress);
    }
}
