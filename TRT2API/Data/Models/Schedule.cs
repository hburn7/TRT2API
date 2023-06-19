namespace TRT2API.Data.Models;

public class Schedule
{
	public int Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public string Type { get; set; } = string.Empty;
	public string Image { get; set; } = string.Empty;
	public int Priority { get; set; }
	public string Link { get; set; } = String.Empty;
	public DateTime Timestamp { get; set; }
}