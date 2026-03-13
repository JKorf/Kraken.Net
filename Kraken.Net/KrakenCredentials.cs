using CryptoExchange.Net.Authentication;

namespace Kraken.Net
{
    /// <summary>
    /// Kraken credentials
    /// </summary>
    public class KrakenCredentials : ApiCredentials
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="apiKey">The API key</param>
        /// <param name="secret">The API secret</param>
        public KrakenCredentials(string apiKey, string secret) : this(new HMACCredential(apiKey, secret)) { }
       
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="credential">The HMAC credentials</param>
        public KrakenCredentials(HMACCredential credential) : base(credential) { }

        /// <inheritdoc />
        public override ApiCredentials Copy() => new KrakenCredentials(Hmac!);
    }
}
