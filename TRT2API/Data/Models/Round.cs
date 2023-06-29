namespace TRT2API.Data.Models;

public class Round
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public int BestOf { get; set; }
	public bool? IsWinnersBracket { get; set; }
}