using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TRT2API.Data;
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
    public object All()
    {
        var dbQuerier = new DbQuerier(_dbSettings.ConnectionString);
        var res = dbQuerier.GetMatches();
        if (!res.Any())
        {
            return BadRequest();
        }

        return res;
    }
    
    /// <summary>
    /// Returns details of a specific match identified by the provided match ID. If no match exists for the ID, it will return a BadRequest.
    /// </summary>
    /// <param name="matchID">ID of the match to retrieve.</param>
    /// <returns>Details of the match or BadRequest if none exist for the provided ID.</returns>
    [HttpGet("{matchID:int}")]
    public object Get(int matchID)
    {
        var dbQuerier = new DbQuerier(_dbSettings.ConnectionString);
        var res = dbQuerier.GetMatch(matchID);
        if (res == null)
        {
            return BadRequest();
        }

        return res;
    }
    
    /// <summary>
    /// Returns a list of players for a specific match identified by the provided match ID. If no players exist for the match, it will return a BadRequest.
    /// </summary>
    /// <param name="matchID">ID of the match to retrieve players for.</param>
    /// <returns>List of players for the match or BadRequest if none exist.</returns>
    [HttpGet("{matchID:int}/players")]
    public object Players(int matchID)
    {
        var dbQuerier = new DbQuerier(_dbSettings.ConnectionString);
        var res = dbQuerier.GetPlayers(matchID);
        
        if (!res.Any())
        {
            return BadRequest();
        }

        return res;
    }
}
