using Microsoft.AspNetCore.Mvc;
using TRT2API.Data.Models;
using TRT2API.Data.Repositories.Interfaces;

namespace TRT2API.Controllers;

[Route("api/maps")]
public class MapsController : ControllerBase
{
	private readonly IDataWorker _dataWorker;
	private readonly ILogger<MapsController> _logger;

	public MapsController(IDataWorker dataWorker, ILogger<MapsController> logger)
	{
		_dataWorker = dataWorker ?? throw new ArgumentNullException(nameof(dataWorker));
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	[HttpGet("all")]
	public async Task<ActionResult<List<Map>>> All()
	{
		var maps = await _dataWorker.Maps.GetAllAsync();
		if (!maps.Any())
		{
			return NotFound("No maps exist.");
		}

		return maps;
	}

	[HttpGet("{osuMapId:long}")]
	public async Task<ActionResult<Map>> Get(long osuMapId)
	{
		var map = await _dataWorker.Maps.GetByOsuMapIdAsync(osuMapId);
		if (map == null)
		{
			return NotFound("No map exists with the provided mapId.");
		}

		return map;
	}

	[HttpPost("add")]
	public async Task<IActionResult> Add([FromBody] Map map)
	{
		if (map == null)
		{
			return BadRequest("Provided map data is null.");
		}

		try
		{
			await _dataWorker.Maps.AddAsync(map);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error when adding map.");
			return Conflict($"There was a conflict when adding the map: {ex}.");
		}

		return Ok(map);
	}

	[HttpPut("{osuMapId:long}")]
	public async Task<IActionResult> Update(long osuMapId, [FromBody] Map map)
	{
		if (map == null)
		{
			return BadRequest("Provided map data is null.");
		}

		if (osuMapId != map.OsuMapId)
		{
			return BadRequest("The mapId in the URL must match the mapId in the provided data.");
		}

		try
		{
			await _dataWorker.Maps.UpdateAsync(map);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error when updating map");
			return BadRequest($"The map you are trying to update does not exist or an error occured: {ex.Message}");
		}

		return NoContent(); // HTTP 204 - success, but no content to return
	}

	[HttpDelete("{osuMapId:long}")]
	public async Task<IActionResult> Delete(long osuMapId)
	{
		try
		{
			var map = await _dataWorker.Maps.GetByOsuMapIdAsync(osuMapId);
			if (map == null)
			{
				return NotFound("No map exists with the provided mapId.");
			}
			
			await _dataWorker.Maps.DeleteAsync(osuMapId);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Error when deleting map with osuMapId {osuMapId}");
			return StatusCode(500, "An error occurred while deleting the map.");
		}

		return NoContent(); // HTTP 204 - success, but no content to return
	}
}