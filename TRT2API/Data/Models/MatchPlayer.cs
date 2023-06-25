namespace TRT2API.Data.Models;

public class MatchPlayer
{
	public int Id { get; set; }
	public int MatchId { get; set; }
	public int PlayerId { get; set; }
	public int? Score { get; set; }
}