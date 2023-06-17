namespace TRT2API.Data.Models;

public class Matchlog
{
	public int ID { get; set; }
	public long PlayerID { get; set; }
	public long[]? Picks { get; set; }
	public long[]? Bans { get; set; }
}