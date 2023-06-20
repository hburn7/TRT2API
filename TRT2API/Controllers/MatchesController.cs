using Microsoft.AspNetCore.Mvc;
using TRT2API.Data.Models;
using TRT2API.Data.Repositories.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace TRT2API.Controllers
{
    public class MatchData
    {
        public Match? Match { get; set; }
        public List<MatchPlayer> MatchPlayers { get; set; } = new();
        public List<MatchMap> MatchMaps { get; set; } = new();
    }

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
        public async Task<ActionResult<List<MatchData>>> All()
        {
            var matches = await _dataWorker.Matches.GetAllAsync();
            if (!matches.Any())
            {
                return BadRequest("No matches exist.");
            }

            var matchDataList = new List<MatchData>();

            foreach(var match in matches)
            {
                var matchPlayers = await _dataWorker.MatchPlayers.GetByMatchIdAsync(match.Id);
                var matchMaps = await _dataWorker.MatchMaps.GetByMatchIdAsync(match.Id);

                matchDataList.Add(new MatchData
                {
                    Match = match,
                    MatchPlayers = matchPlayers,
                    MatchMaps = matchMaps
                });
            }

            return matchDataList;
        }

        [HttpGet("{matchID:int}")]
        public async Task<ActionResult<MatchData>> Get(long matchID)
        {
            var match = await _dataWorker.Matches.GetByMatchIdAsync(matchID);
            if(match == null)
            {
                return NotFound("No such match exists.");
            }

            var matchPlayers = await _dataWorker.MatchPlayers.GetByMatchIdAsync(matchID);
            var matchMaps = await _dataWorker.MatchMaps.GetByMatchIdAsync(matchID);

            return new MatchData
            {
                Match = match,
                MatchPlayers = matchPlayers,
                MatchMaps = matchMaps
            };
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] MatchData matchData)
        {
            if (matchData == null || matchData.Match == null)
            {
                return BadRequest("Provided match data is null.");
            }

            try
            {
                // Add the Match
                var addedMatch = await _dataWorker.Matches.AddAsync(matchData.Match);

                // Add the MatchPlayers
                foreach (var matchPlayer in matchData.MatchPlayers)
                {
                    matchPlayer.MatchId = addedMatch.Id;
                    await _dataWorker.MatchPlayers.AddAsync(matchPlayer);
                }

                // Add the MatchMaps
                foreach (var matchMap in matchData.MatchMaps)
                {
                    matchMap.MatchId = addedMatch.Id;
                    await _dataWorker.MatchMaps.AddAsync(matchMap);
                }
            }
            catch (Exception e)
            {
                return BadRequest("Unable to add the match. " + e.Message);
            }

            return NoContent(); // HTTP 204 - success, but no content to return
        }
        
        [HttpPatch("{matchId:long}/{playerId:long}/incrementscore")]
        public async Task<IActionResult> IncrementScore(long matchId, long playerId)
        {
            try
            {
                await _dataWorker.MatchPlayers.IncrementScoreAsync(matchId, playerId);
            }
            catch (Exception e)
            {
                return BadRequest("Unable to increment the match player's score. " + e.Message);
            }

            return NoContent(); // HTTP 204 - success, but no content to return
        }
    }
}
