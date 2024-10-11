using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// Order result
    /// </summary>
    public record KrakenOrderResult
    {
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("cl_ord_id")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// User reference id
        /// </summary>
        [JsonPropertyName("order_userref")]
        public long? UserReference { get; set; }
        /// <summary>
        /// Error
        /// </summary>
        [JsonPropertyName("error")]
        public string? Error { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }
}
