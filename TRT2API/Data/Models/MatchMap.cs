namespace TRT2API.Data.Models;

public class MatchMap
{
	public int Id { get; set; }
	public long MatchId { get; set; }
	public long PlayerId { get; set; }
	public long MapId { get; set; }
	public string Action { get; set; } = string.Empty;
	public int OrderInMatch { get; set; }
}