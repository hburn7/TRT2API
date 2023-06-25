namespace TRT2API.Data.Models;

public class Match
{
	public int Id { get; set; }
	public long? OsuMatchId { get; set; }
	public string Type { get; set; } = string.Empty;
	public int ScheduleId { get; set; }
	public long? WinnerId { get; set; }
	public DateTime? TimeStart { get; set; }
	public DateTime? LastUpdated { get; set; }
	public int? BracketMatchId { get; set; }
}