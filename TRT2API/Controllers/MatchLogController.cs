using Microsoft.AspNetCore.Mvc;
using TRT2API.Data.Models;
using TRT2API.Data.Repositories.Interfaces;

namespace TRT2API.Controllers;

[Route("api/matchlog")]
public class MatchLogController : Controller
{
	private readonly IDataWorker _dataWorker;
	private readonly ILogger<MatchLogController> _logger;
	
	public MatchLogController(IDataWorker dataWorker, ILogger<MatchLogController> logger)
	{
		_dataWorker = dataWorker;
		_logger = logger;
	}
	
	[HttpGet("all")]
	public async Task<List<MatchMap>> All() => await _dataWorker.MatchMaps.GetAllAsync();
	
	[HttpGet("{matchID:int}")]
	public async Task<List<MatchMap>> Get(int matchID) => await _dataWorker.MatchMaps.GetByMatchIdAsync(matchID);
	
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
}