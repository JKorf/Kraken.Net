using CryptoExchange.Net.Authentication;

namespace Kraken.Net
{
    /// <summary>
    /// Kraken credentials
    /// </summary>
    public class KrakenCredentials : ApiCredentials
    {
        /// <summary>
        /// </summary>
        [Obsolete("Parameterless constructor is only for deserialization purposes and should not be used directly. Use parameterized constructor instead.")]
        public KrakenCredentials() { }

        /// <summary>
        /// Create credentials using an HMAC key and secret
        /// </summary>
        /// <param name="apiKey">The API key</param>
        /// <param name="secret">The API secret</param>
        public KrakenCredentials(string apiKey, string secret) : this(new HMACCredential(apiKey, secret)) { }

        /// <summary>
        /// Create Kraken credentials using HMAC credentials
        /// </summary>
        /// <param name="credential">The HMAC credentials</param>
        public KrakenCredentials(HMACCredential credential) : base(credential) { }

        /// <inheritdoc />
#pragma warning disable CS0618 // Type or member is obsolete
        public override ApiCredentials Copy() => new KrakenCredentials { CredentialPairs = CredentialPairs };
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
