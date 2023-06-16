using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
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
	
	// GET api/players/{playerID}
	[HttpGet("{playerID}")]
	public object Get(long playerID)
	{
		var dbQuerier = new DbQuerier(_dbSettings.ConnectionString);
		var res = dbQuerier.GetPlayer(playerID);
		if (res == null)
		{
			return BadRequest();
		}

		return res;
	}
	
	// GET api/players/{playerID}/matches
	[HttpGet("{playerID}/matches")]
	public object Matches(long playerID)
	{
		var dbQuerier = new DbQuerier(_dbSettings.ConnectionString);
		var res = dbQuerier.GetMatches().Where(m => m.Player1ID == playerID || m.Player2ID == playerID).ToList();

		if (!res.Any())
		{
			return BadRequest();
		}

		return res;
	}
}