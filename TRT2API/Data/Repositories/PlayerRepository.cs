﻿using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using TRT2API.Data.Models;
using TRT2API.Data.Repositories.Interfaces;
using TRT2API.Settings;

namespace TRT2API.Data.Repositories;

public class PlayerRepository : IPlayerRepository
{
	private readonly string _connectionString;
	private readonly ILogger<PlayerRepository> _logger;

	public PlayerRepository(IOptions<DatabaseSettings> dbSettings, ILogger<PlayerRepository> logger)
	{
		_connectionString = dbSettings.Value.ConnectionString;
		_logger = logger;
	}

	public async Task<List<Player>> GetAllAsync()
	{
		const string sql = "SELECT * FROM players";

		try
		{
			using var connection = new NpgsqlConnection(_connectionString);
			return (await connection.QueryAsync<Player>(sql)).ToList();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error getting all players");
			throw;
		}
	}

	public async Task<Player> AddAsync(Player player)
	{
		const string sql = @"
            INSERT INTO players(osuplayerid, name, totalmatches, totalwins, status, iseliminated, seeding)
            VALUES(@OsuPlayerId, @Name, @TotalMatches, @TotalWins, @Status, @IsEliminated, @Seeding)
            RETURNING *;";

		try
		{
			using var connection = new NpgsqlConnection(_connectionString);
			return await connection.QuerySingleAsync<Player>(sql, player);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error adding a player");
			throw;
		}
	}

	public async Task<Player> UpdateAsync(Player player)
	{
		const string sql = @"
            UPDATE players SET 
            osuplayerid = @OsuPlayerId, 
            name = @Name, 
            totalmatches = @TotalMatches, 
            totalwins = @TotalWins, 
            status = @Status, 
            iseliminated = @IsEliminated, 
            seeding = @Seeding 
            WHERE id = @Id 
            RETURNING *;";

		try
		{
			using var connection = new NpgsqlConnection(_connectionString);
			return await connection.QuerySingleAsync<Player>(sql, player);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error updating a player");
			throw;
		}
	}

	public async Task<Player> DeleteAsync(long osuPlayerId)
	{
		const string sql = "DELETE FROM players WHERE osuplayerid = @OsuPlayerId RETURNING *;";

		try
		{
			using var connection = new NpgsqlConnection(_connectionString);
			return await connection.QuerySingleAsync<Player>(sql, new { OsuPlayerId = osuPlayerId });
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error deleting a player");
			throw;
		}
	}

	public async Task<Player> GetByOsuPlayerIdAsync(long osuPlayerId)
	{
		const string sql = "SELECT * FROM players WHERE osuplayerid = @PlayerId;";

		try
		{
			using var connection = new NpgsqlConnection(_connectionString);
			return await connection.QuerySingleOrDefaultAsync<Player>(sql, new { PlayerId = osuPlayerId });
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Error getting player with id {osuPlayerId}");
			throw;
		}
	}

	public async Task<List<Player>> GetByMatchIdAsync(int matchId)
	{
		const string sql = @"
            SELECT p.*
            FROM players p
            INNER JOIN match_players mp ON p.id = mp.playerid
            WHERE mp.matchid = @MatchId;";

		try
		{
			using var connection = new NpgsqlConnection(_connectionString);
			var result = await connection.QueryAsync<Player>(sql, new { MatchId = matchId });
			return result.ToList();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Error getting players for match {matchId}");
			throw;
		}
	}

	public async Task<Player> IncrementWinsAsync(long osuPlayerId)
	{
		const string sql = @"
			UPDATE players SET 
			totalwins = totalwins + 1
			WHERE osuplayerid = @OsuPlayerId
			RETURNING *;";

		try
		{
			using var connection = new NpgsqlConnection(_connectionString);
			return await connection.QuerySingleAsync<Player>(sql, new { OsuPlayerId = osuPlayerId });
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Error incrementing wins for player {osuPlayerId}");
			throw;
		}
	}
}