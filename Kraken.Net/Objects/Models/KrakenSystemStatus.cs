using Kraken.Net.Enums;
using Kraken.Net.Objects.Models.Socket;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// System status
    /// </summary>
    [SerializationModel]
    public record KrakenSystemStatus
    {
        /// <summary>
        /// ["<c>status</c>"] Platform status
        /// </summary>

        [JsonPropertyName("status")]
        public SystemStatus Status { get; set; }
        /// <summary>
        /// ["<c>timestamp</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>upcoming_maintenance</c>"] Upcoming maintenances
        /// </summary>
        [JsonPropertyName("upcoming_maintenance")]
        public KrakenPlannedMaintenance[] UpcomingMaintenance { get; set; } = [];
        /// <summary>
        /// ["<c>emergency</c>"] Incidents
        /// </summary>
        [JsonPropertyName("emergency")]
        public KrakenIncident[] Incidents { get; set; } = [];
    }
}
