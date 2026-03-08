namespace Kraken.Net.Objects.Models
{
	/// <summary>
	/// Order info
	/// </summary>
    [SerializationModel]
	public record KrakenWithdraw
	{
		/// <summary>
		/// ["<c>refid</c>"] Reference id
		/// </summary>
		[JsonPropertyName("refid")]
		public string ReferenceId { get; set; } = string.Empty;
	}
}
