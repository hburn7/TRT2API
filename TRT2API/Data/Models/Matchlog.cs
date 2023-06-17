namespace TRT2API.Data.Models;

public class Matchlog
{
	public long PlayerID { get; set; }
	public long[]? Picks { get; set; }
	public long[]? Bans { get; set; }
}