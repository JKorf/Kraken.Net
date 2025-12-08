namespace Kraken.Net.Objects.Models.Futures
{
    [SerializationModel]
    internal record KrakenFuturesPlatfromNotificationInternalResult : KrakenFuturesResult
    {
        [JsonPropertyName("notifications")]
        public KrakenFuturesPlatfromNotification[] Notifications { get; set; } = Array.Empty<KrakenFuturesPlatfromNotification>();
    }

    /// <summary>
    /// Platform info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesPlatfromNotificationResult
    {
        /// <summary>
        /// Notifications
        /// </summary>
        [JsonPropertyName("notifications")]
        public KrakenFuturesPlatfromNotification[] Notifications { get; set; } = Array.Empty<KrakenFuturesPlatfromNotification>();
        /// <summary>
        /// Server time
        /// </summary>
        [JsonPropertyName("serverTime")]
        public DateTime ServerTime { get; set; }
    }

    /// <summary>
    /// Platform notification info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesPlatfromNotification
    {
        /// <summary>
        /// The time that notification is taking effect.
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("effectiveTime")]
        public DateTime? EffectiveTime { get; set; }
        /// <summary>
        /// The notification note. A short description about the specific notification.
        /// </summary>
        [JsonPropertyName("note")]
        public string? Note { get; set; }
        /// <summary>
        /// The notification priorities: low / medium / high. If priority == "high" then it implies downtime will occur at EffectiveTime when Type == "maintenance".
        /// </summary>
        [JsonPropertyName("priority")]
        public string Priority { get; set; } = string.Empty;
        /// <summary>
        /// If type == "maintenance" then it implies downtime will occur at EffectiveTime if Priority == "high".
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }
}
