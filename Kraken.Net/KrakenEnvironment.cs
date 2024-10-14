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

        /// <summary>
        /// Futures Rest client address
        /// </summary>
        public string FuturesRestBaseAddress { get; }

        /// <summary>
        /// Futures Socket client address
        /// </summary>
        public string FuturesSocketBaseAddress { get; }

        internal KrakenEnvironment(string name,
            string spotRestBaseAddress,
            string spotSocketPublicAddress,
            string spotSocketPrivateAddress,
            string futuresRestBaseAddress,
            string futuresSocketBaseAddress) : base(name)
        {
            SpotRestBaseAddress = spotRestBaseAddress;
            SpotSocketPublicAddress = spotSocketPublicAddress;
            SpotSocketPrivateAddress = spotSocketPrivateAddress;
            FuturesRestBaseAddress = futuresRestBaseAddress;
            FuturesSocketBaseAddress = futuresSocketBaseAddress;
        }

        /// <summary>
        /// Live environment
        /// </summary>
        public static KrakenEnvironment Live { get; }
            = new KrakenEnvironment(TradeEnvironmentNames.Live,
                                   KrakenApiAddresses.Default.SpotRestClientAddress,
                                   KrakenApiAddresses.Default.SpotSocketPublicAddress,
                                   KrakenApiAddresses.Default.SpotSocketPrivateAddress,
                                   KrakenApiAddresses.Default.FuturesRestClientAddress,
                                   KrakenApiAddresses.Default.FuturesSocketClientAddress);

        /// <summary>
        /// Create a custom environment
        /// </summary>
        /// <param name="name"></param>
        /// <param name="spotRestAddress"></param>
        /// <param name="spotSocketPublicAddress"></param>
        /// <param name="spotSocketPrivateAddress"></param>
        /// <param name="futuresRestAddress"></param>
        /// <param name="futuresSocketAddress"></param>
        /// <returns></returns>
        public static KrakenEnvironment CreateCustom(
                        string name,
                        string spotRestAddress,
                        string spotSocketPublicAddress,
                        string spotSocketPrivateAddress,
                        string futuresRestAddress,
                        string futuresSocketAddress)
            => new KrakenEnvironment(name, spotRestAddress, spotSocketPublicAddress, spotSocketPrivateAddress, futuresRestAddress, futuresSocketAddress);
    }
}
