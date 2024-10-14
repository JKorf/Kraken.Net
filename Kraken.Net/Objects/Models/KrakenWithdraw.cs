namespace Kraken.Net.Objects.Models
{
	/// <summary>
	/// Order info
	/// </summary>
	public record KrakenWithdraw
	{
		/// <summary>
		/// Reference id
		/// </summary>
		[JsonPropertyName("refid")]
		public string ReferenceId { get; set; } = string.Empty;
	}
}