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
	public DbQuerier(string connectionString)
	{
		_connectionString = connectionString;
	}
	
	public List<Player> GetPlayers()
	{
		using (var connection = new NpgsqlConnection(_connectionString))
		{
			const string sql = "SELECT * FROM players;";
			return connection.Query<Player>(sql).ToList();
		}
	}
	
	public List<Match> GetMatches()
	{
		using (var connection = new NpgsqlConnection(_connectionString))
		{
			const string sql = "SELECT * FROM matches;";
			return connection.Query<Match>(sql).ToList();
		}
	}
	
	public Player? GetPlayer(long playerID)
	{
		using (var connection = new NpgsqlConnection(_connectionString))
		{
			const string sql = "SELECT * FROM players WHERE player_id = @PlayerID;";
			return connection.Query<Player?>(sql, new { PlayerID = playerID }).FirstOrDefault();
		}
	}
	
	public Match? GetMatch(long matchID)
	{
		using (var connection = new NpgsqlConnection(_connectionString))
		{
			const string sql = "SELECT * FROM matches WHERE match_id = @MatchID;";
			return connection.Query<Match?>(sql, new { MatchID = matchID }).FirstOrDefault();
		}
	}
}