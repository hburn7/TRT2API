﻿using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using TRT2API.Data.Models;
using TRT2API.Data.Repositories.Interfaces;
using TRT2API.Settings;

namespace TRT2API.Data.Repositories
{
    public class MatchPlayerRepository : IMatchPlayerRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<MatchPlayerRepository> _logger;

        public MatchPlayerRepository(IOptions<DatabaseSettings> dbSettings, ILogger<MatchPlayerRepository> logger)
        {
            _connectionString = dbSettings.Value.ConnectionString;
            _logger = logger;
        }

        public async Task<List<MatchPlayer>> GetAllAsync()
        {
            const string sql = "SELECT * FROM match_players";

            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                return (await connection.QueryAsync<MatchPlayer>(sql)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all match players");
                throw;
            }
        }

        public async Task<MatchPlayer> AddAsync(MatchPlayer matchPlayer)
        {
            const string sql = @"
                INSERT INTO match_players(matchid, playerid, playerscore)
                VALUES(@MatchId, @PlayerId, @Score)
                RETURNING *;";

            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                return await connection.QuerySingleAsync<MatchPlayer>(sql, matchPlayer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding a match player");
                throw;
            }
        }

        public async Task<MatchPlayer> UpdateAsync(MatchPlayer matchPlayer)
        {
            const string sql = @"
                UPDATE match_players SET 
                matchid = @MatchId, 
                playerid = @PlayerId, 
                playerscore = @Score
                WHERE id = @Id 
                RETURNING *;";

            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                return await connection.QuerySingleAsync<MatchPlayer>(sql, matchPlayer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating a match player");
                throw;
            }
        }

        public async Task<MatchPlayer> DeleteAsync(MatchPlayer matchPlayer)
        {
            const string sql = "DELETE FROM match_players WHERE id = @Id RETURNING *;";

            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                return await connection.QuerySingleAsync<MatchPlayer>(sql, new { matchPlayer.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting a match player");
                throw;
            }
        }

        public async Task<List<MatchPlayer>> GetByMatchIdAsync(int matchId)
        {
            const string sql = "SELECT * FROM match_players WHERE matchid = @MatchId;";

            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                return (await connection.QueryAsync<MatchPlayer>(sql, new { MatchId = matchId })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting match player with match id {matchId}");
                throw;
            }
        }
    }
}
