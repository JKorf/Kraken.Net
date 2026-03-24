namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// API key info
    /// </summary>
    public record KrakenApiKey
    {
        /// <summary>
        /// ["<c>api_key_name</c>"] Api key name
        /// </summary>
        [JsonPropertyName("api_key_name")]
        public string ApiKeyName { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>api_key</c>"] Api key
        /// </summary>
        [JsonPropertyName("api_key")]
        public string ApiKey { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>nonce</c>"] Nonce
        /// </summary>
        [JsonPropertyName("nonce")]
        public long Nonce { get; set; }
        /// <summary>
        /// ["<c>nonce_window</c>"] Nonce window
        /// </summary>
        [JsonPropertyName("nonce_window")]
        public int NonceWindow { get; set; }
        /// <summary>
        /// ["<c>permissions</c>"] Permissions
        /// </summary>
        [JsonPropertyName("permissions")]
        public string[] Permissions { get; set; } = [];
        /// <summary>
        /// ["<c>iban</c>"] Iban
        /// </summary>
        [JsonPropertyName("iban")]
        public string Iban { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>valid_until</c>"] Valid until
        /// </summary>
        [JsonPropertyName("valid_until")]
        public DateTime? ValidUntil { get; set; }
        /// <summary>
        /// ["<c>query_from</c>"] Query from
        /// </summary>
        [JsonPropertyName("query_from")]
        public DateTime? QueryFrom { get; set; }
        /// <summary>
        /// ["<c>query_to</c>"] Query to
        /// </summary>
        [JsonPropertyName("query_to")]
        public DateTime? QueryTo { get; set; }
        /// <summary>
        /// ["<c>created_time</c>"] Create time
        /// </summary>
        [JsonPropertyName("created_time")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// ["<c>modified_time</c>"] Update time
        /// </summary>
        [JsonPropertyName("modified_time")]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// ["<c>ip_allowlist</c>"] Ip allowlist
        /// </summary>
        [JsonPropertyName("ip_allowlist")]
        public string[] IpAllowlist { get; set; } = [];
        /// <summary>
        /// ["<c>last_used</c>"] Last used
        /// </summary>
        [JsonPropertyName("last_used")]
        public DateTime LastUsed { get; set; }
    }
}
