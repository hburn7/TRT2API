namespace TRT2API.Data.Models;

public class MatchMap
{
	public int Id { get; set; }
	public int MatchId { get; set; }
	public int PlayerId { get; set; }
	public int MapId { get; set; }
	public string Action { get; set; } = string.Empty;
	public int OrderInMatch { get; set; }
}