namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// Socket token
    /// </summary>
    [SerializationModel]
    public record KrakenWebSocketToken
    {
        /// <summary>
        /// ["<c>token</c>"] Token to use for connecting to private websockets
        /// </summary>
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>expires</c>"] Expires after x seconds
        /// </summary>
        [JsonPropertyName("expires")]
        public int Expires { get; set; }
    }
}
