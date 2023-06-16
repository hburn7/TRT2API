using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TRT2API.Data;
using TRT2API.Data.Models;
using TRT2API.Settings;

namespace TRT2API.Controllers;

[Route("api/players")]
public class PlayerController : ControllerBase
{
	private readonly DatabaseSettings _dbSettings;
	public PlayerController(IOptions<DatabaseSettings> dbSettings)
	{
		_dbSettings = dbSettings.Value;
	}
	
	// GET api/players/all
	[HttpGet("all")]
	public List<Player> All()
	{
		var dbQuerier = new DbQuerier(_dbSettings.ConnectionString);
		return dbQuerier.GetPlayers();
	}
}