using Microsoft.AspNetCore.Mvc;
using TRT2API.Data.Models;
using TRT2API.Data.Repositories.Interfaces;

namespace TRT2API.Controllers;

[Route("api/matchplayers")]
public class MatchPlayerController : ControllerBase
{
	private readonly ILogger<MatchPlayerController> _logger;
	private readonly IDataWorker _dataWorker;
	
	public MatchPlayerController(ILogger<MatchPlayerController> logger, IDataWorker dataWorker)
	{
		_logger = logger;
		_dataWorker = dataWorker;
	}

	[HttpGet("all")]
	public async Task<ActionResult<List<MatchPlayer>>> All()
	{
		var matchPlayers = await _dataWorker.MatchPlayers.GetAllAsync();
		if (matchPlayers == null || !matchPlayers.Any())
		{
			return NotFound("No MatchPlayer objects in database.");
		}
		
		return Ok(matchPlayers);
	}
	
	[HttpGet("{id:int}")]
	public async Task<ActionResult<MatchPlayer>> Get(int id)
	{
		var matchPlayers = await _dataWorker.MatchPlayers.GetAsync(id);
		if (matchPlayers == null)
		{
			return NotFound($"No MatchPlayer objects with id {id} in database.");
		}
		
		return Ok(matchPlayers);
	}
	
	[HttpPost("add")]
	public async Task<ActionResult<MatchPlayer>> Add([FromBody] MatchPlayer matchPlayer)
	{
		if (matchPlayer == null)
		{
			return BadRequest();
		}
		
		await _dataWorker.MatchPlayers.AddAsync(matchPlayer);
		return NoContent();
	}
	
	[HttpDelete("delete/{id:int}")]
	public async Task<ActionResult<MatchPlayer>> Delete(int id)
	{
		var matchPlayer = await _dataWorker.MatchPlayers.GetByMatchIdAsync(id);
		if (matchPlayer == null)
		{
			return NotFound();
		}
		
		await _dataWorker.MatchPlayers.DeleteAsync(id);
		return NoContent();
	}
	
	[HttpPut("update/{id:int}")]
	public async Task<ActionResult<MatchPlayer>> Update(int id, [FromBody] MatchPlayer matchPlayer)
	{
		if (matchPlayer == null)
		{
			return BadRequest();
		}

		if (id != matchPlayer.Id)
		{
			return BadRequest("Id mismatch.");
		}
		
		var matchPlayerToUpdate = await _dataWorker.MatchPlayers.GetAsync(id);
		if (matchPlayerToUpdate == null)
		{
			return NotFound();
		}
		
		await _dataWorker.MatchPlayers.UpdateAsync(matchPlayer);
		return NoContent();
	}
}