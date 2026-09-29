using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// System status
    /// </summary>
    [SerializationModel]
    public record KrakenStreamSystemStatus
    {
        /// <summary>
        /// ["<c>connection_id</c>"] Connection id
        /// </summary>
        [JsonPropertyName("connection_id")]
        public ulong ConnectionId { get; set; }
        /// <summary>
        /// ["<c>system</c>"] Status
        /// </summary>
        [JsonPropertyName("system")]
        public SystemStatus Status { get; set; }
        /// <summary>
        /// ["<c>version</c>"] Version
        /// </summary>
        [JsonPropertyName("version")]
        public string Version { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>api_version</c>"] API Version
        /// </summary>
        [JsonPropertyName("api_version")]
        public string ApiVersion { get; set; } = string.Empty;
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

    /// <summary>
    /// Maintenance
    /// </summary>
    public record KrakenPlannedMaintenance
    {
        /// <summary>
        /// ["<c>event_id</c>"] Event id
        /// </summary>
        [JsonPropertyName("event_id")]
        public long EventId { get; set; }
        /// <summary>
        /// ["<c>title</c>"] Title
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>expected_start_utc</c>"] Expected start time
        /// </summary>
        [JsonPropertyName("expected_start_utc")]
        public DateTime ExpectedStart { get; set; }
        /// <summary>
        /// ["<c>expected_end_utc</c>"] Expected end time
        /// </summary>
        [JsonPropertyName("expected_end_utc")]
        public DateTime? ExpectedEnd { get; set; }
        /// <summary>
        /// ["<c>time_to_start_s</c>"] Time to start in seconds
        /// </summary>
        [JsonPropertyName("time_to_start_s")]
        public int TimeToStart { get; set; }
        /// <summary>
        /// ["<c>phase</c>"] Phase
        /// </summary>
        [JsonPropertyName("phase")]
        public string Phase { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>affected_services</c>"] Affected services
        /// </summary>
        [JsonPropertyName("affected_services")]
        public string[] AffectedServices { get; set; } = [];
        /// <summary>
        /// ["<c>order_submission</c>"] Order submission
        /// </summary>
        [JsonPropertyName("order_submission")]
        public string OrderSubmission { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>recommended_action</c>"] Recommended action
        /// </summary>
        [JsonPropertyName("recommended_action")]
        public string RecommendedAction { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>cancel_before_utc</c>"] Time before which orders can be canceled
        /// </summary>
        [JsonPropertyName("cancel_before_utc")]
        public DateTime? CancelBefore { get; set; }
        /// <summary>
        /// ["<c>source_url</c>"] Source URL
        /// </summary>
        [JsonPropertyName("source_url")]
        public string SourceUrl { get; set; } = string.Empty;
    }

    /// <summary>
    /// Incident
    /// </summary>
    public record KrakenIncident
    {
        /// <summary>
        /// ["<c>event_id</c>"] Event id
        /// </summary>
        [JsonPropertyName("event_id")]
        public long EventId { get; set; }
        /// <summary>
        /// ["<c>title</c>"] Title
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>incident_status</c>"] Incident status
        /// </summary>
        [JsonPropertyName("incident_status")]
        public string IncidentStatus { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>impact</c>"] Impact
        /// </summary>
        [JsonPropertyName("impact")]
        public string Impact { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>affected_services</c>"] Affected services
        /// </summary>
        [JsonPropertyName("affected_services")]
        public string[] AffectedServices { get; set; } = [];
        /// <summary>
        /// ["<c>started_at_utc</c>"] Started at time
        /// </summary>
        [JsonPropertyName("started_at_utc")]
        public DateTime StartedAt { get; set; }
        /// <summary>
        /// ["<c>next_steps</c>"] Expected next steps
        /// </summary>
        [JsonPropertyName("next_steps")]
        public KrakenIncidentStep[] NextSteps { get; set; } = [];
        /// <summary>
        /// ["<c>source_url</c>"] Source URL
        /// </summary>
        [JsonPropertyName("source_url")]
        public string SourceUrl { get; set; } = string.Empty;
    }

    /// <summary>
    /// Incident step
    /// </summary>
    public record KrakenIncidentStep
    {
        /// <summary>
        /// ["<c>applies_to</c>"] Applies to
        /// </summary>
        [JsonPropertyName("applies_to")]
        public string[] AppliesTo { get; set; } = [];
        /// <summary>
        /// ["<c>type</c>"] Type
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>expected_at_utc</c>"] Expected time
        /// </summary>
        [JsonPropertyName("expected_at_utc")]
        public DateTime? ExpectedAt { get; set; }
    }
}
