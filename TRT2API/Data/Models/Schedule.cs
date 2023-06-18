namespace TRT2API.Data.Models;

public class Schedule
{
	public int Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public DateTime Timestamp { get; set; }
}