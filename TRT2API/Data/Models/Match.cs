namespace TRT2API.Data.Models;

public class Match
{
	public int ID { get; set; }
	public long? MatchID { get; set; }
	public long[] PlayerIDs { get; set; }
	public long? WinnerID { get; set; }
}