namespace TRT2API.Data.Models;

public class Match
{
	public int Id { get; set; }
	public long? MatchId { get; set; }
	public string Type { get; set; } = string.Empty;
	public int ScheduleId { get; set; }
	public long? WinnerId { get; set; }
	public DateTime? TimeStart { get; set; }
	public DateTime? LastUpdated { get; set; }
	public int[] PlayerIds { get; set; } = Array.Empty<int>();
	public int[] MapIds { get; set; } = Array.Empty<int>();
	public int? BracketMatchId { get; set; }
}