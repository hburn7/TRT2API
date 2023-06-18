using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TRT2API.Data;
using TRT2API.Data.Models;
using TRT2API.Settings;

namespace TRT2API.Controllers;

/// <summary>
/// Controller for managing Match-related API endpoints.
/// </summary>
[Route("api/matches")]
public class MatchesController : ControllerBase
{
    private readonly DatabaseSettings _dbSettings;
    
    public MatchesController(IOptions<DatabaseSettings> dbSettings)
    {
        _dbSettings = dbSettings.Value;
    }
    
    /// <summary>
    /// Returns a list of all matches. If there are no matches, it will return a BadRequest.
    /// </summary>
    /// <returns>List of all matches or BadRequest if none exist.</returns>
    [HttpGet("all")]
    public async Task<ActionResult<List<Match>>> All()
    {
        var dbQuerier = new DbQuerier(_dbSettings.ConnectionString);
        var res = await dbQuerier.GetMatchesAsync();
        if (!res.Any())
        {
            return BadRequest("No matches exist.");
        }

        return res;
    }
    
    /// <summary>
    /// Returns details of a specific match identified by the provided match ID. If no match exists for the ID, it will return a BadRequest.
    /// </summary>
    /// <param name="matchID">ID of the match to retrieve.</param>
    /// <returns>Details of the match or BadRequest if none exist for the provided ID.</returns>
    [HttpGet("{matchID:int}")]
    public async Task<ActionResult<Match>> Get(int matchID)
    {
        var dbQuerier = new DbQuerier(_dbSettings.ConnectionString);
        var res = await dbQuerier.GetMatchAsync(matchID);
        if (res == null)
        {
            return BadRequest("No match exists for the provided ID.");
        }

        return res;
    }

    /// <summary>
    /// Returns a list of players for a specific match identified by the provided match ID. If no players exist for the match, it will return a BadRequest.
    /// </summary>
    /// <param name="matchID">ID of the match to retrieve players for.</param>
    /// <returns>List of players for the match or BadRequest if none exist.</returns>
    [HttpGet("{matchID:int}/players")]
    public async Task<ActionResult<List<Player>>> Players(int matchID)
    {
        var dbQuerier = new DbQuerier(_dbSettings.ConnectionString);
        var res = await dbQuerier.GetPlayersAsync(matchID);
        
        if (!res.Any())
        {
            return BadRequest("No players played in this match or the match does not exist.");
        }

        return res;
    }
    
    // === PUT ===
    // PUT api/matches/{matchID}
    [HttpPut("{matchID:long}")]
    public async Task<ActionResult> Update(long matchID, [FromBody] Match match)
    {
        if (matchID != match.MatchID)
        {
            return BadRequest("The matchID in the URL must match the matchID in the provided data.");
        }

        var dbQuerier = new DbQuerier(_dbSettings.ConnectionString);
        int affectedRows = await dbQuerier.UpdateMatchAsync(match);

        if (affectedRows == 0)
        {
            return NotFound("No match found with the provided MatchID.");
        }

        return NoContent(); // HTTP 204 - success, but no content to return
    }
}
