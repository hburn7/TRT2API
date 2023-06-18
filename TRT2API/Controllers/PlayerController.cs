using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TRT2API.Data;
using TRT2API.Data.Models;
using TRT2API.Settings;
using System.Threading.Tasks;

namespace TRT2API.Controllers
{
    [Route("api/players")]
    public class PlayerController : ControllerBase
    {
        private readonly DatabaseSettings _dbSettings;

        public PlayerController(IOptions<DatabaseSettings> dbSettings)
        {
            _dbSettings = dbSettings.Value;
        }

        // GET api/players/all
        [HttpGet("all")]
        public async Task<ActionResult<List<Player>>> All()
        {
            var dbQuerier = new DbQuerier(_dbSettings.ConnectionString);
            var res = await dbQuerier.GetPlayersAsync();
            
            if (!res.Any())
            {
                return BadRequest();
            }

            return res;
        }

        // GET api/players/{playerID}
        [HttpGet("{playerID:long}")]
        public async Task<ActionResult<Player>> Get(long playerID)
        {
            var dbQuerier = new DbQuerier(_dbSettings.ConnectionString);
            var res = await dbQuerier.GetPlayerAsync(playerID);
            
            if (res == null)
            {
                return BadRequest();
            }

            return res;
        }

        // GET api/players/{playerID}/matches
        [HttpGet("{playerID:long}/matches")]
        public async Task<ActionResult<List<Match>>> Matches(long playerID)
        {
            var dbQuerier = new DbQuerier(_dbSettings.ConnectionString);
            var res = await dbQuerier.GetMatchesAsync(playerID);
            
            if (!res.Any())
            {
                return BadRequest();
            }

            return res;
        }

        [HttpPost("add")]
        public async Task<ActionResult<int>> Add([FromBody] Player player)
        {
            var dbQuerier = new DbQuerier(_dbSettings.ConnectionString);

            try
            {
               return await dbQuerier.AddPlayerAsync(player);
            }
            catch (Exception)
            {
                return Conflict();
            }
        }
        
        [HttpPost("update")]
        public async Task<ActionResult<int>> Update([FromBody] Player player)
        {
            var dbQuerier = new DbQuerier(_dbSettings.ConnectionString);
            try
            {
                return await dbQuerier.UpdatePlayerAsync(player);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
