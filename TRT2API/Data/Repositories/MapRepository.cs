using Dapper;
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
            INSERT INTO maps (mapid, round, mod, postmodsr, metadata)
            VALUES (@MapId, @Round, @Mod, @PostModSr, @Metadata)
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

	public async Task<Map> UpdateAsync(Map map)
	{
		const string sql = @"
            UPDATE maps
            SET mapid = @MapId, 
                round = @Round, 
                mod = @Mod, 
                postmodsr = @PostModSr, 
                metadata = @Metadata
            WHERE mapid = @MapId;";

		try
		{
			using var connection = new NpgsqlConnection(_connectionString);
			int affectedRows = await connection.ExecuteAsync(sql, map);
			if (affectedRows > 0)
			{
				return map;
			}

			throw new Exception($"No map found with mapId {map.MapId} to update.");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Error updating map {map.MapId}");
			throw;
		}
	}

	public async Task<Map> DeleteAsync(Map map)
	{
		const string sql = "DELETE FROM maps WHERE mapid = @MapId;";
		try
		{
			using var connection = new NpgsqlConnection(_connectionString);
			int affectedRows = await connection.ExecuteAsync(sql, new
			{
				map.MapId
			});

			if (affectedRows > 0)
			{
				return map;
			}

			throw new Exception($"No map found with id {map.Id} to delete.");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Error deleting map {map.Id}");
			throw;
		}
	}

	public async Task<Map> GetByMapIdAsync(long mapId)
	{
		const string sql = "SELECT * FROM maps WHERE mapid = @MapId";

		try
		{
			using var connection = new NpgsqlConnection(_connectionString);
			return await connection.QuerySingleAsync<Map>(sql, new { MapId = mapId });
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Error getting map with id {mapId}");
			throw;
		}
	}
}