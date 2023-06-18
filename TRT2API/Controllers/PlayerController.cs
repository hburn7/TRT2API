using Microsoft.AspNetCore.Mvc;
using TRT2API.Data;
using TRT2API.Data.Models;
using System.Linq;
using Microsoft.Extensions.Logging;
// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

namespace TRT2API.Controllers
{
    [Route("api/players")]
    public class PlayerController : ControllerBase
    {
        private readonly DbQuerier _dbQuerier;
        private readonly ILogger<PlayerController> _logger;

        public PlayerController(DbQuerier dbQuerier, ILogger<PlayerController> logger)
        {
            _dbQuerier = dbQuerier ?? throw new ArgumentNullException(nameof(dbQuerier));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Player>>> All()
        {
            var players = await _dbQuerier.GetPlayersAsync();
            if (!players.Any())
            {
                return NotFound("No players exist.");
            }

            return players;
        }

        [HttpGet("{playerID:long}")]
        public async Task<ActionResult<Player>> Get(long playerID)
        {
            var player = await _dbQuerier.GetPlayerAsync(playerID);
            if (player == null)
            {
                return NotFound("No player exists for the provided ID.");
            }

            return player;
        }

        [HttpGet("{playerID:long}/matches")]
        public async Task<ActionResult<List<Match>>> Matches(long playerID)
        {
            var matches = await _dbQuerier.GetMatchesAsync(playerID);
            if (!matches.Any())
            {
                return NotFound("No matches found for the provided playerID.");
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
               int playerId = await _dbQuerier.AddPlayerAsync(player);
               return Ok(playerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when adding player.");
                return Conflict("There was a conflict when adding the player.");
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] Player player)
        {
            if (player == null)
            {
                return BadRequest("Provided player data is null.");
            }

            try
            {
                int affectedRows = await _dbQuerier.UpdatePlayerAsync(player);
                if (affectedRows == 0)
                {
                    return NotFound("No player found with the provided PlayerID.");
                }
                
                return NoContent(); // HTTP 204 - success, but no content to return
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when updating player.");
                return StatusCode(500, "An error occurred while updating the player.");
            }
        }
    }
}
