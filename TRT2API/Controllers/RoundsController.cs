using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using TRT2API.Data.Models;
using TRT2API.Data.Repositories.Interfaces;

namespace TRT2API.Controllers;

[Route("api/rounds")]
public class RoundsController : Controller
{
	private readonly IDataWorker _dataWorker;
	private readonly ILogger<RoundsController> _logger;

	public RoundsController(IDataWorker dataWorker, ILogger<RoundsController> logger)
	{
		_dataWorker = dataWorker ?? throw new ArgumentNullException(nameof(dataWorker));
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	[HttpGet("all")]
	public async Task<ActionResult<List<Round>?>> GetAll()
	{
		try
		{
			var rounds = await _dataWorker.Rounds.GetAllAsync();
			if (!rounds?.Any() ?? true)
			{
				return NotFound("No matches found in the database.");
			}

			return rounds;
		}
		catch (Exception e)
		{
			return StatusCode(500, $"An error occurred while retrieving the rounds: {e}");
		}
	}

	[HttpGet("{name}")]
	public async Task<ActionResult<Round?>> Get(string name)
	{
		try
		{
			var round = await _dataWorker.Rounds.GetAsync(name);
			if (round == null)
			{
				return NotFound("There is no match for the provided round name.");
			}

			return round;
		}
		catch (Exception e)
		{
			return StatusCode(500, $"An error occurred while retrieving the round: {e}");
		}
	}
}