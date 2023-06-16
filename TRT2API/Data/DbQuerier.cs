using Dapper;
using Npgsql;
using TRT2API.Data.Models;

namespace TRT2API.Data;

/// <summary>
/// Responsible for all of the program's database queries.
/// </summary>
public class DbQuerier
{
	private readonly string? _connectionString;
	public DbQuerier()
	{
		if ((_connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")) == null)
		{
			throw new Exception("Missing connection string: DB_CONNECTION_STRING env var is not present.");
		}
		
		_connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
	}
	
	public List<Player> GetPlayers()
	{
		using (var connection = new NpgsqlConnection(_connectionString))
		{
			const string sql = "SELECT * FROM players;";
			return connection.Query<Player>(sql).ToList();
		}
	}
}