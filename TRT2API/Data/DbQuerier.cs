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

	public List<Player> GetPlayers(int matchID)
	{
		using (var connection = new NpgsqlConnection(_connectionString))
		{
			const string sql = @"SELECT p.* 
                             FROM players p 
                             JOIN matches m ON p.playerid = ANY(m.playerids) 
                             WHERE m.matchid = @MatchID;";
			return connection.Query<Player>(sql, new { MatchID = matchID }).ToList();
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

	/// <summary>
	/// Returns all matches that the player has participated in.
	/// </summary>
	/// <param name="playerID"></param>
	/// <returns></returns>
	public List<Match> GetMatches(long playerID)
	{
		using (var connection = new NpgsqlConnection(_connectionString))
		{
			const string sql = "SELECT *, array_to_string(playerids, ',') AS player_ids_string FROM matches WHERE playerids @> ARRAY[@PlayerID];";
        
			// Dapper doesn't know how to map PostgreSQL arrays to C#, so we have
			// to do that ourselves.
			var matches = connection.Query<Match, string, Match>(sql,
				(match, player_ids_string) =>
				{
					if (!string.IsNullOrEmpty(player_ids_string))
					{
						match.PlayerIDs = player_ids_string.Split(',')
						                                   .Where(p => !string.IsNullOrEmpty(p))
						                                   .Select(long.Parse)
						                                   .ToArray();
					}
					else
					{
						match.PlayerIDs = Array.Empty<long>();
					}

					return match;
				},
				new { PlayerID = playerID },
				splitOn: "player_ids_string");

			return matches.ToList();
		}
	}

	
	public Player? GetPlayer(long playerID)
	{
		using (var connection = new NpgsqlConnection(_connectionString))
		{
			const string sql = "SELECT * FROM players WHERE playerid = @PlayerID;";
			return connection.Query<Player?>(sql, new { PlayerID = playerID }).FirstOrDefault();
		}
	}
	
	public Match? GetMatch(long matchID)
	{
		using (var connection = new NpgsqlConnection(_connectionString))
		{
			const string sql = "SELECT * FROM matches WHERE matchid = @MatchID;";
			return connection.Query<Match?>(sql, new { MatchID = matchID }).FirstOrDefault();
		}
	}
}