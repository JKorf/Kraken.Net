using CryptoExchange.Net.Authentication;
using System.Net;

namespace Kraken.Net
{
    /// <summary>
    /// Kraken credentials
    /// </summary>
    public class KrakenCredentials : ApiCredentials
    {
        /// <summary>
        /// Spot credentials
        /// </summary>
        public HMACCredential? Spot { get; set; }
        /// <summary>
        /// Futures credentials
        /// </summary>
        public HMACCredential? Futures { get; set; }

        /// <summary>
        /// Create new credentials
        /// </summary>
        public KrakenCredentials() { }

        /// <summary>
        /// </summary>
        [Obsolete("Use the constructor that takes separate credentials for spot and futures instead")]
        public KrakenCredentials(string apiKey, string secret) 
        {
            // Sets the same credentials for both spot and futures, which doesn't make much sense but is kept for backwards compatibility
            Spot = new HMACCredential(apiKey, secret);
            Futures = new HMACCredential(apiKey, secret);
        }

        /// <summary>
        /// Create new credentials providing HMAC credentials
        /// </summary>
        /// <param name="spotCredential">HMAC credentials for the spot API</param>
        /// <param name="futuresCredential">HMAC credentials for the futures API</param>
        public KrakenCredentials(HMACCredential? spotCredential, HMACCredential? futuresCredential)
        {
            Spot = spotCredential;
            Futures = futuresCredential;
        }

        /// <summary>
        /// Specify the Spot HMAC credentials
        /// </summary>
        /// <param name="key">API key</param>
        /// <param name="secret">API secret</param>
        public KrakenCredentials WithSpotHMAC(string key, string secret)
        {
            if (Spot != null) throw new InvalidOperationException("Spot credentials already set");

            Spot = new HMACCredential(key, secret);
            return this;
        }

        /// <summary>
        /// Specify the Futures HMAC credentials
        /// </summary>
        /// <param name="key">API key</param>
        /// <param name="secret">API secret</param>
        public KrakenCredentials WithFuturesHMAC(string key, string secret)
        {
            if (Futures != null) throw new InvalidOperationException("Futures credentials already set");

            Futures = new HMACCredential(key, secret);
            return this;
        }

        /// <inheritdoc />
        public override ApiCredentials Copy() => new KrakenCredentials { Spot = Spot, Futures = Futures };

        /// <inheritdoc />
        public override void Validate()
        {
            Spot?.Validate();
            Futures?.Validate();
        }
    }
}
