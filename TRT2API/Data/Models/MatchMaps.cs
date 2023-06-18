namespace TRT2API.Data.Models;

public class MatchMaps
{
	public int Id { get; set; }
	public long MatchId { get; set; }
	public long MapId { get; set; }
	public bool? IsPicked { get; set; }
	public bool? IsBanned { get; set; }
	public int? PickOrder { get; set; }
	public int? BanOrder { get; set; }
}