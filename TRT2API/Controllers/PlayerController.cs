using Microsoft.AspNetCore.Mvc;
using TRT2API.Data;
using TRT2API.Data.Models;

namespace TRT2API.Controllers;

[Route("api/players")]
public class PlayerController : ControllerBase
{
	// GET api/players
	[HttpGet]
	public List<Player> All()
	{
		var dbQuerier = new DbQuerier();
		return dbQuerier.GetPlayers();
	}
}