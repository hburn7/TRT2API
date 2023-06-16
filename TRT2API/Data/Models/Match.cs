namespace TRT2API.Data.Models;

public class Match
{
	public int ID { get; set; }
	public long MatchID { get; set; }
	public long Player1ID { get; set; }
	public long Player2ID { get; set; }
	public long WinnerID { get; set; }
}