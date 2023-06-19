using System.Text.Json;

namespace TRT2API.Data.Models;

public class Map 
{
	public int Id { get; set; }
	public long MapId { get; set; }
	public string Round { get; set; } = null!;
	public string Mod { get; set; } = null!;
	public double PostModSr { get; set; }
	public JsonDocument Metadata { get; set; } = null!;
}
