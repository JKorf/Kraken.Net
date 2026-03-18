using CryptoExchange.Net.Authentication;

namespace Kraken.Net
{
    /// <summary>
    /// Kraken credentials
    /// </summary>
    public class KrakenCredentials : ApiCredentials
    {
        public HMACCredential? Spot { get; set; }
        public HMACCredential? Futures { get; set; }

        public KrakenCredentials WithSpot(string key, string secret)
        {
            if (Spot != null) throw new InvalidOperationException("Spot credentials already set");

            Spot = new HMACCredential(key, secret);
            return this;
        }

        public KrakenCredentials WithFutures(string key, string secret)
        {
            if (Futures != null) throw new InvalidOperationException("Futures credentials already set");

            Futures = new HMACCredential(key, secret);
            return this;
        }

        /// <inheritdoc />
        public override ApiCredentials Copy() => new KrakenCredentials { Spot = Spot, Futures = Futures };
    }
}
