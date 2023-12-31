﻿using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using TRT2API.Data.Models;
using TRT2API.Data.Repositories.Interfaces;
using TRT2API.Settings;

namespace TRT2API.Data.Repositories;

public class MatchRepository : IMatchRepository
{
	private readonly string _connectionString;
	private readonly ILogger<MatchRepository> _logger;

	public MatchRepository(IOptions<DatabaseSettings> dbSettings, ILogger<MatchRepository> logger)
	{
		_connectionString = dbSettings.Value.ConnectionString;
		_logger = logger;
	}

	public async Task<List<Match>> GetAllAsync()
	{
		const string sql = "SELECT * FROM matches;";
		try
		{
			using var connection = new NpgsqlConnection(_connectionString);
			var result = await connection.QueryAsync<Match>(sql);
			return result.ToList();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error getting all matches");
			throw;
		}
	}

	public async Task<Match> AddAsync(Match match)
	{
		const string sql = @"
            INSERT INTO matches (osumatchid, type, scheduleid, winnerid, timestart, lastupdated, bracketmatchid, roundid)
            VALUES (@OsuMatchId, @Type, @ScheduleId, @WinnerId, @TimeStart, @LastUpdated, @BracketMatchId, @RoundId)
            RETURNING id;";

		try
		{
			using var connection = new NpgsqlConnection(_connectionString);
			match.Id = await connection.QuerySingleAsync<int>(sql, match);
			return match;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error adding new match");
			throw;
		}
	}

	public async Task<Match> UpdateAsync(Match match)
	{
		const string sql = @"
            UPDATE matches
            SET osumatchid = @OsuMatchId, 
                type = @Type, 
                scheduleid = @ScheduleId, 
                winnerid = @WinnerId,
                timestart = @TimeStart, 
                lastupdated = @LastUpdated,
                bracketmatchid = @BracketMatchId,
                roundid = @RoundId
            WHERE id = @Id;";

		try
		{
			using var connection = new NpgsqlConnection(_connectionString);
			int affectedRows = await connection.ExecuteAsync(sql, match);
			if (affectedRows > 0)
			{
				return match;
			}

			throw new Exception($"No match found with id {match.Id} to update.");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Error updating match {match.Id}");
			throw;
		}
	}

	public async Task<Match> DeleteAsync(Match match)
	{
		const string sql = "DELETE FROM matches WHERE id = @Id;";
		try
		{
			using var connection = new NpgsqlConnection(_connectionString);
			int affectedRows = await connection.ExecuteAsync(sql, new
			{
				match.Id
			});

			if (affectedRows > 0)
			{
				return match;
			}

			throw new Exception($"No match found with id {match.Id} to delete.");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Error deleting match {match.Id}");
			throw;
		}
	}

	public async Task<List<Match>> GetByPlayerIdAsync(int playerId)
	{
		const string sql = @"
        SELECT m.* 
        FROM matches m 
        INNER JOIN match_players mp ON m.id = mp.matchid
        WHERE mp.playerid = @PlayerId;";

		try
		{
			using var connection = new NpgsqlConnection(_connectionString);
			var result = await connection.QueryAsync<Match>(sql, new { PlayerId = playerId });
			return result.ToList();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Error getting matches for player {playerId}");
			throw;
		}
	}

	public async Task<Match> GetByOsuMatchIdAsync(long matchId)
	{
		const string sql = "SELECT * FROM matches WHERE osumatchid = @OsuMatchId";

		try
		{
			using var connection = new NpgsqlConnection(_connectionString);
			return await connection.QuerySingleOrDefaultAsync<Match>(sql, new { OsuMatchId = matchId });
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Error getting match with osumatchid {matchId}");
			throw;
		}
	}

	// public async Task<List<Player>> GetPlayersForMatchIdAsync(long matchId)
	// {
	// 	const string sql = @"
 //        SELECT P.* 
 //        FROM players P 
 //        INNER JOIN match_players MP ON P.id = MP.playerid 
 //        WHERE MP.matchid = @MatchId";
 //
	// 	try
	// 	{
	// 		using var connection = new NpgsqlConnection(_connectionString);
	// 		var players = await connection.QueryAsync<Player>(sql, new { MatchId = matchId });
	// 		return players.ToList();
	// 	}
	// 	catch (Exception ex)
	// 	{
	// 		_logger.LogError(ex, $"Error getting players for match id {matchId}");
	// 		throw;
	// 	}
	// }

	public async Task<Match?> GetAsync(int id)
	{
		const string sql = "SELECT * FROM matches WHERE id = @Id";
		try
		{
			using var connection = new NpgsqlConnection(_connectionString);
			return await connection.QuerySingleAsync<Match?>(sql, new { Id = id });
		}
		catch (Exception e)
		{
			_logger.LogError(e, $"Error getting match with id {id}");
			return await Task.FromException<Match?>(e);
		}
	}
}