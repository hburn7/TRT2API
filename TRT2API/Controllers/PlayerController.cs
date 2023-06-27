using Microsoft.AspNetCore.Mvc;
using TRT2API.Data.Models;
using TRT2API.Data.Repositories.Interfaces;

namespace TRT2API.Controllers
{
	[Route("api/players")]
	public class PlayerController : ControllerBase
	{
		private readonly IDataWorker _dataWorker;
		private readonly ILogger<PlayerController> _logger;

		public PlayerController(IDataWorker dataWorker, ILogger<PlayerController> logger)
		{
			_dataWorker = dataWorker ?? throw new ArgumentNullException(nameof(dataWorker));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		[HttpGet("all")]
		public async Task<ActionResult<List<Player>>> All()
		{
			var players = await _dataWorker.Players.GetAllAsync();
			if (!players.Any())
			{
				return NotFound("No players exist.");
			}

			return players;
		}

		[HttpGet("{osuPlayerId:long}")]
		public async Task<ActionResult<Player>> Get(long osuPlayerId)
		{
			var player = await _dataWorker.Players.GetByOsuPlayerIdAsync(osuPlayerId);
			if (player == null)
			{
				return NotFound("No player exists for the provided ID.");
			}

			return player;
		}

		[HttpGet("{playerId:long}/matches")]
		public async Task<ActionResult<List<Match>>> Matches(int playerId)
		{
			var matches = await _dataWorker.Matches.GetByPlayerIdAsync(playerId);
			if (!matches.Any())
			{
				return NotFound("No matches found for the provided playerId.");
			}

			return matches;
		}

		[HttpPost("add")]
		public async Task<IActionResult> Add([FromBody] Player player)
		{
			if (player == null)
			{
				return BadRequest("Provided player data is null.");
			}

			try
			{
				var addedPlayer = await _dataWorker.Players.AddAsync(player);
				return Ok(addedPlayer.OsuPlayerId);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error when adding player.");
				return Conflict("There was a conflict when adding the player.");
			}
		}

		[HttpPut("{osuPlayerID:long}")]
		public async Task<IActionResult> Update(long osuPlayerID, [FromBody] Player player)
		{
			if (player == null)
			{
				return BadRequest("Provided player data is null.");
			}

			if (osuPlayerID != player.OsuPlayerId)
			{
				return BadRequest("The playerID in the URL must match the playerID in the provided data.");
			}

			try
			{
				await _dataWorker.Players.UpdateAsync(player);
				return NoContent(); // HTTP 204 - success, but no content to return
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error when updating player.");
				return StatusCode(500, "An error occurred while updating the player.");
			}
		}
		
		[HttpPut("{osuPlayerId:long}/incrementwins")]
		public async Task<IActionResult> AddWin(long osuPlayerId)
		{
			try
			{
				await _dataWorker.Players.IncrementWinsAsync(osuPlayerId);
				return NoContent(); // HTTP 204 - success, but no content to return
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error when updating player (increment wins).");
				return StatusCode(500, "An error occurred while updating the player.");
			}
		}
	}
}