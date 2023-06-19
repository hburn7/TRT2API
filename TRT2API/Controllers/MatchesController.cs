using Microsoft.AspNetCore.Mvc;
using TRT2API.Data.Models;
using TRT2API.Data.Repositories.Interfaces;

namespace TRT2API.Controllers;

[Route("api/matches")]
public class MatchesController : ControllerBase
{
	private readonly IDataWorker _dataWorker;
	private readonly ILogger<MatchesController> _logger;

	public MatchesController(IDataWorker dataWorker, ILogger<MatchesController> logger)
	{
		_dataWorker = dataWorker;
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	[HttpGet("all")]
	public async Task<ActionResult<List<Match>>> All()
	{
		var matches = await _dataWorker.Matches.GetAllAsync();
		if (!matches.Any())
		{
			return BadRequest("No matches exist.");
		}

		return matches;
	}

	[HttpGet("{matchID:int}")]
	public async Task<ActionResult<Match>> Get(int matchID) => await _dataWorker.Matches.GetByMatchIdAsync(matchID);

	[HttpGet("{matchID:int}/players")]
	public async Task<ActionResult<List<Player>>> Players(int matchID)
	{
		var players = await _dataWorker.Matches.GetPlayersForMatchIdAsync(matchID);
		if (!players.Any())
		{
			return NotFound("No players played in this match or the match does not exist.");
		}

		return players;
	}

	[HttpPut("{matchID:int}")]
	public async Task<IActionResult> Update(int matchID, [FromBody] Match? match)
	{
		if (match == null)
		{
			return BadRequest("Provided match data is null.");
		}

		if (matchID != match.Id)
		{
			return BadRequest("The matchID in the URL must match the matchID in the provided data.");
		}

		try
		{
			await _dataWorker.Matches.UpdateAsync(match);
		}
		catch (Exception e)
		{
			return BadRequest("Unable to update the match. " + e.Message);
		}

		return NoContent(); // HTTP 204 - success, but no content to return
	}

	[HttpPost("add")]
	public async Task<IActionResult> Add([FromBody] Match match)
	{
		if (match == null)
		{
			return BadRequest("Provided match data is null.");
		}

		try
		{
			await _dataWorker.Matches.AddAsync(match);
		}
		catch (Exception e)
		{
			return BadRequest("Unable to add the match. " + e.Message);
		}

		return NoContent(); // HTTP 204 - success, but no content to return
	}
}