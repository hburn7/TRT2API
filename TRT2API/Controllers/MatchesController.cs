using Microsoft.AspNetCore.Mvc;
using TRT2API.Data;
using TRT2API.Data.Models;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace TRT2API.Controllers
{
    [Route("api/matches")]
    public class MatchesController : ControllerBase
    {
        private readonly DbQuerier _dbQuerier;
        private readonly ILogger<MatchesController> _logger;
        
        public MatchesController(DbQuerier dbQuerier, ILogger<MatchesController> logger)
        {
            _dbQuerier = dbQuerier ?? throw new ArgumentNullException(nameof(dbQuerier));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Match>>> All()
        {
            var matches = await _dbQuerier.GetMatchesAsync();
            if (!matches.Any())
            {
                return BadRequest("No matches exist.");
            }

            return matches;
        }

        [HttpGet("{matchID:int}")]
        public async Task<ActionResult<Match>> Get(int matchID)
        {
            var match = await _dbQuerier.GetMatchAsync(matchID);
            if (match == null)
            {
                return NotFound("No match exists for the provided ID.");
            }

            return match;
        }

        [HttpGet("{matchID:int}/players")]
        public async Task<ActionResult<List<Player>>> Players(int matchID)
        {
            var players = await _dbQuerier.GetPlayersAsync(matchID);
            if (!players.Any())
            {
                return NotFound("No players played in this match or the match does not exist.");
            }

            return players;
        }

        [HttpPut("{matchID:int}")]
        public async Task<IActionResult> Update(int matchID, [FromBody] Match match)
        {
            if (match == null)
            {
                return BadRequest("Provided match data is null.");
            }

            if (matchID != match.Id)
            {
                return BadRequest("The matchID in the URL must match the matchID in the provided data.");
            }

            int affectedRows = await _dbQuerier.UpdateMatchAsync(match);

            if (affectedRows == 0)
            {
                return NotFound("No match found with the provided MatchID.");
            }

            return NoContent(); // HTTP 204 - success, but no content to return
        }
    }
}
