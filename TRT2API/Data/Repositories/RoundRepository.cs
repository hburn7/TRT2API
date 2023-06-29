using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using TRT2API.Data.Models;
using TRT2API.Data.Repositories.Interfaces;
using TRT2API.Settings;

namespace TRT2API.Data.Repositories;

public class RoundRepository : IRoundRepository
{
	private readonly string _connectionString;
	private readonly ILogger<RoundRepository> _logger;

	public RoundRepository(IOptions<DatabaseSettings> dbSettings, ILogger<RoundRepository> logger)
	{
		_connectionString = dbSettings.Value.ConnectionString;
		_logger = logger;
	}

	public async Task<List<Round>?> GetAllAsync()
	{
		try
		{
			const string sql = "SELECT * FROM rounds;";
			using var connection = new NpgsqlConnection(_connectionString);
			return (await connection.QueryAsync<Round>(sql)).ToList();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error getting all rounds");
			return null;
		}
	}

	public async Task<List<Round>?> GetAsync(string name)
	{
		try
		{
			const string sql = "SELECT * FROM rounds WHERE name = @Name;";
			using var connection = new NpgsqlConnection(_connectionString);
			return (await connection.QueryAsync<Round>(sql, new {Name = name})).ToList();
		}
		catch (Exception e)
		{
			_logger.LogError(e, "Error getting round by name");
			return null;
		}
	}
}