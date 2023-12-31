﻿using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Npgsql;
using TRT2API.Data.Models;
using TRT2API.Data.Repositories.Interfaces;
using TRT2API.Settings;

namespace TRT2API.Data.Repositories;

public class MapRepository : IMapRepository
{
	private readonly string _connectionString;
	private readonly ILogger<MapRepository> _logger;

	public MapRepository(IOptions<DatabaseSettings> dbSettings, ILogger<MapRepository> logger)
	{
		_connectionString = dbSettings.Value.ConnectionString;
		_logger = logger;
	}

	public async Task<List<Map>> GetAllAsync()
	{
		const string sql = "SELECT * FROM maps;";
		try
		{
			using var connection = new NpgsqlConnection(_connectionString);
			var result = await connection.QueryAsync<Map>(sql);
			return result.ToList();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error getting all maps");
			throw;
		}
	}

	public async Task<Map> AddAsync(Map map)
	{
		const string sql = @"
            INSERT INTO maps (osumapid, round, mod, postmodsr, metadata)
            VALUES (@OsuMapId, @Round, @Mod, @PostModSr, @Metadata)
            RETURNING id;";

		try
		{
			using var connection = new NpgsqlConnection(_connectionString);
			map.Id = await connection.QuerySingleAsync<int>(sql, map);
			return map;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error adding new map");
			throw;
		}
	}

	public async Task<Map?> UpdateAsync(Map map)
	{
		const string sql = @"
            UPDATE maps
            SET mod = @Mod,
                postmodsr = @PostModSr,
                metadata = @Metadata
            WHERE osumapid = @OsuMapId and round = @Round;";

		try
		{
			using var connection = new NpgsqlConnection(_connectionString);
			int affectedRows = await connection.ExecuteAsync(sql, map);
			if (affectedRows > 0)
			{
				return map;
			}

			_logger.LogWarning($"No map found with osumapid {map.OsuMapId} to update for round {map.Round}");
			return null;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Error updating map {map.OsuMapId} for round {map.Round}");
			return null;
		}
	}

	public async Task DeleteAsync(string round, long osuMapId)
	{
		const string sql = "DELETE FROM maps WHERE osumapid = @OsuMapId AND round = @Round;";
		try
		{
			using var connection = new NpgsqlConnection(_connectionString);
			int affectedRows = await connection.ExecuteAsync(sql, new
			{
				OsuMapId = osuMapId,
				Round = round
			});

			if (affectedRows > 0)
			{
				return;
			}

			throw new Exception($"No map found with osumapid {osuMapId} for round {round} to delete.");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Error deleting map with osumapid {osuMapId}");
			throw;
		}
	}

	public async Task<Map?> GetByOsuMapIdAndRoundAsync(long osuMapId, string round)
	{
		const string sql = "SELECT * FROM maps WHERE osumapid = @OsuMapId and round = @Round";

		try
		{
			using var connection = new NpgsqlConnection(_connectionString);
			return await connection.QuerySingleAsync<Map>(sql, new { OsuMapId = osuMapId, Round = round });
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Error getting map with id {osuMapId} for round {round}");
			return null;
		}
	}
}