using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using TRT2API.Data.Models;
using TRT2API.Data.Repositories.Interfaces;
using TRT2API.Settings;

namespace TRT2API.Data.Repositories
{
    public class MatchMapRepository : IMatchMapRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<MatchMapRepository> _logger;

        public MatchMapRepository(IOptions<DatabaseSettings> dbSettings, ILogger<MatchMapRepository> logger)
        {
            _connectionString = dbSettings.Value.ConnectionString;
            _logger = logger;
        }

        public async Task<List<MatchMap>> GetAllAsync()
        {
            const string sql = "SELECT * FROM match_maps";

            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                return (await connection.QueryAsync<MatchMap>(sql)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all match maps");
                throw;
            }
        }

        public async Task<MatchMap> AddAsync(MatchMap matchMap)
        {
            const string sql = @"
                INSERT INTO match_maps(matchid, playerid, mapid, action, orderinmatch)
                VALUES(@MatchId, @PlayerId, @MapId, @Action, @OrderInMatch)
                RETURNING *;";

            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                return await connection.QuerySingleAsync<MatchMap>(sql, matchMap);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding a match map");
                throw;
            }
        }

        public async Task<MatchMap> UpdateAsync(MatchMap matchMap)
        {
            const string sql = @"
                UPDATE match_maps SET 
                matchid = @MatchId, 
                playerid = @PlayerId, 
                mapid = @MapId,
                action = @Action,
                orderinmatch = @OrderInMatch
                WHERE id = @Id 
                RETURNING *;";

            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                return await connection.QuerySingleAsync<MatchMap>(sql, matchMap);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating a match map");
                throw;
            }
        }

        public async Task<MatchMap> DeleteAsync(MatchMap matchMap)
        {
            const string sql = "DELETE FROM match_maps WHERE id = @Id RETURNING *;";

            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                return await connection.QuerySingleAsync<MatchMap>(sql, new { matchMap.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting a match map");
                throw;
            }
        }

        public Task<MatchMap> GetAsync(int id)
        {
            const string sql = "SELECT * FROM match_maps WHERE id = @Id;";
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                return connection.QuerySingleAsync<MatchMap>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting match map with id {id}");
                throw;
            }
        }

        public async Task<List<MatchMap>> GetByMatchIdAsync(int matchId)
        {
            const string sql = "SELECT * FROM match_maps WHERE matchid = @MatchId;";

            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                return (await connection.QueryAsync<MatchMap>(sql, new { MatchId = matchId })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting match map with match id {matchId}");
                throw;
            }
        }

        public Task DeleteAsync(int id)
        {
            const string sql = "DELETE FROM match_maps WHERE id = @Id;";
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                return connection.ExecuteAsync(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting match map with id {id}");
                throw;
            }
        }
    }
}
