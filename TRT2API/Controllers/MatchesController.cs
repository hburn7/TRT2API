using Microsoft.AspNetCore.Mvc;
using TRT2API.Data.Models;
using TRT2API.Data.Repositories.Interfaces;

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
            if (!matches?.Any() ?? true)
            {
                return NotFound("No matches exist.");
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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MatchData>> Get(int id)
        {
            try
            {
                var match = await _dataWorker.Matches.GetAsync(id);
                if(match == null)
                {
                    return NotFound("No such match exists.");
                }

                var matchPlayers = await _dataWorker.MatchPlayers.GetByMatchIdAsync(id);
                var matchMaps = await _dataWorker.MatchMaps.GetByMatchIdAsync(id);

                return new MatchData
                {
                    Match = match,
                    MatchPlayers = matchPlayers,
                    MatchMaps = matchMaps
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting match by id");
                return StatusCode(500, "Match not found or error occurred.");
            }
        }
        
        [HttpGet("osumatch/{osuMatchId:long}")]
        public async Task<ActionResult<MatchData>> GetByOsuMatchId(long osuMatchId)
        {
            try
            {
                var match = await _dataWorker.Matches.GetByOsuMatchIdAsync(osuMatchId);
                if(match == null)
                {
                    return NotFound("No such match exists.");
                }

                var matchPlayers = await _dataWorker.MatchPlayers.GetByMatchIdAsync(match.Id);
                var matchMaps = await _dataWorker.MatchMaps.GetByMatchIdAsync(match.Id);

                return new MatchData
                {
                    Match = match,
                    MatchPlayers = matchPlayers,
                    MatchMaps = matchMaps
                };
            }
            catch (Exception e)
            {
                _logger.LogError($"Error getting match by osu match id: {osuMatchId}", e);
                return StatusCode(500, $"Error getting match by osu match id: {osuMatchId}");
            }
            
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddMatchData([FromBody] MatchData matchData)
        {
            if (matchData == null || matchData.Match == null)
            {
                return BadRequest("Provided match data is null.");
            }

            try
            {
                if (matchData.Match == null)
                {
                    return BadRequest("Could not parse a match object from the provided data.");
                }
                // Add the Match
                var addedMatch = await _dataWorker.Matches.AddAsync(matchData.Match);

                if (addedMatch == null)
                {
                    return BadRequest("An error occurred while adding the match.");
                }

                if (matchData.MatchPlayers != null)
                {
                    // Add the MatchPlayers
                    foreach (var matchPlayer in matchData.MatchPlayers)
                    {
                        matchPlayer.MatchId = addedMatch.Id;
                        await _dataWorker.MatchPlayers.AddAsync(matchPlayer);
                    }
                }

                if (matchData.MatchMaps != null)
                {
                    // Add the MatchMaps
                    foreach (var matchMap in matchData.MatchMaps)
                    {
                        matchMap.MatchId = addedMatch.Id;
                        await _dataWorker.MatchMaps.AddAsync(matchMap);
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, "Unable to add the match due to error: " + e.Message);
            }

            return NoContent(); // HTTP 204 - success, but no content to return
        }
    }
}
