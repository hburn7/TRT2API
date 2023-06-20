namespace TRT2API.Data.Models;

public class Player
{
	public int Id { get; set; }
	public long PlayerId { get; set; }
	public string PlayerName { get; set; } = string.Empty;
	public int TotalMatches { get; set; }
	public int TotalWins { get; set; }
	public string Status { get; set; } = string.Empty;
	public bool IsEliminated { get; set; }
	public int? Seeding { get; set; }
}