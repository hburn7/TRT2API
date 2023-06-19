namespace TRT2API.Data.Models;

public class MatchPlayer
{
	public int Id { get; set; }
	public long MatchId { get; set; }
	public long PlayerId { get; set; }
	public int? Score { get; set; }
}