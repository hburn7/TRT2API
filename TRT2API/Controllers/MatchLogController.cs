using Microsoft.AspNetCore.Mvc;
using TRT2API.Data.Models;
using TRT2API.Data.Repositories.Interfaces;

namespace TRT2API.Controllers;

[Route("api/matchlog")]
public class MatchLogController : ControllerBase
{
	private readonly IDataWorker _dataWorker;
	private readonly ILogger<MatchLogController> _logger;
	
	public MatchLogController(IDataWorker dataWorker, ILogger<MatchLogController> logger)
	{
		_dataWorker = dataWorker;
		_logger = logger;
	}
	
	[HttpGet("all")]
	public async Task<ActionResult<List<MatchMap>>> All()
	{
		var data = await _dataWorker.MatchMaps.GetAllAsync();
		if (data == null || !data.Any())
		{
			return NotFound("No MatchMap objects in database.");
		}
		
		return Ok(data);
	}
	
	[HttpGet("{id:int}")]
	public async Task<MatchMap> Get(int id) => await _dataWorker.MatchMaps.GetAsync(id);
	
	[HttpPost("add")]
	public async Task<ActionResult<MatchMap>> Add([FromBody] MatchMap matchMap)
	{
		if (matchMap == null)
		{
			return BadRequest();
		}
		
		await _dataWorker.MatchMaps.AddAsync(matchMap);
		return NoContent();
	}
	
	[HttpDelete("delete/{matchId:int}")]
	public async Task<ActionResult<MatchMap>> Delete(int matchId)
	{
		var matchMap = await _dataWorker.MatchMaps.GetByMatchIdAsync(matchId);
		if (matchMap == null)
		{
			return NotFound();
		}
		
		await _dataWorker.MatchMaps.DeleteAsync(matchId);
		return NoContent();
	}
	
	[HttpPut("update/{matchId:int}")]
	public async Task<ActionResult<MatchMap>> Update(int matchId, [FromBody] MatchMap matchMap)
	{
		if (matchMap == null || matchMap.Id != matchId)
		{
			return BadRequest();
		}
		
		var matchMapToUpdate = await _dataWorker.MatchMaps.GetByMatchIdAsync(matchId);
		if (matchMapToUpdate == null)
		{
			return NotFound();
		}
		
		await _dataWorker.MatchMaps.UpdateAsync(matchMap);
		return NoContent();
	}
}