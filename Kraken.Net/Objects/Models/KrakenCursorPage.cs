using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Data page with a cusor for pagination
    /// </summary>
    public record KrakenCursorPage<T>
    {
        /// <summary>
        /// Cursor for the next page
        /// </summary>
        [JsonPropertyName("next_cursor")]
        public string? NextCursor { get; set; }

        /// <summary>
        /// Page data
        /// </summary>
        [JsonPropertyName("items")]
        public IEnumerable<T> Items { get; set; } = Array.Empty<T>();
        [JsonPropertyName("withdrawals")]
        internal IEnumerable<T> Withdrawals { set => Items = value; }
        [JsonPropertyName("deposits")]
        internal IEnumerable<T> Deposits { set => Items = value; }
    }
}
