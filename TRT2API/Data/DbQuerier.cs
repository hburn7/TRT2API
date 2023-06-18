using Dapper;
using Npgsql;
using TRT2API.Data.Models;
using TRT2API.Settings;

namespace TRT2API.Data;

/// <summary>
///  Responsible for all of the program's database queries.
/// </summary>
public class DbQuerier
{
	private readonly string? _connectionString;
	private readonly ILogger<DbQuerier> _logger;

	public DbQuerier(DatabaseSettings dbSettings, ILogger<DbQuerier> logger)
	{
		_connectionString = dbSettings.ConnectionString;
		_logger = logger;
	}

	public async Task<List<Player>> GetPlayersAsync()
	{
		using (var connection = new NpgsqlConnection(_connectionString))
		{
			try
			{
				const string sql = "SELECT * FROM players;";
				var result = await connection.QueryAsync<Player>(sql);
				return result.ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting players");
				throw;
			}
		}
	}

	public async Task<List<Player>> GetPlayersAsync(int matchID)
	{
		using (var connection = new NpgsqlConnection(_connectionString))
		{
			try
			{
				const string sql = @"SELECT p.* 
                                FROM players p 
                                JOIN match_players mp ON p.id = mp.player_id
                                WHERE mp.match_id = @MatchID;";

				var result = await connection.QueryAsync<Player>(sql, new { MatchID = matchID });
				return result.ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error getting players for match ID {matchID}");
				throw;
			}
		}
	}

	public async Task<List<Match>> GetMatchesAsync()
	{
		using (var connection = new NpgsqlConnection(_connectionString))
		{
			try
			{
				const string sql = "SELECT * FROM matches;";
				var result = await connection.QueryAsync<Match>(sql);
				return result.ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting matches");
				throw;
			}
		}
	}

	public async Task<List<Match>> GetMatchesAsync(long playerID)
	{
		using (var connection = new NpgsqlConnection(_connectionString))
		{
			try
			{
				const string sql = @"SELECT m.* 
								 FROM matches m 
								 JOIN match_players mp ON m.id = mp.match_id 
								 WHERE mp.player_id = @PlayerID;";

				var result = await connection.QueryAsync<Match>(sql, new { PlayerID = playerID });
				return result.ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error getting matches for player with ID {playerID}");
				throw;
			}
		}
	}

	public async Task<Player?> GetPlayerAsync(long playerID)
	{
		using (var connection = new NpgsqlConnection(_connectionString))
		{
			try
			{
				const string sql = "SELECT * FROM players WHERE player_id = @PlayerID;";
				var result = await connection.QueryFirstOrDefaultAsync<Player?>(sql, new { PlayerID = playerID });
				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error getting player with ID {playerID}");
				throw;
			}
		}
	}

	public async Task<Match?> GetMatchAsync(int matchID)
	{
		using (var connection = new NpgsqlConnection(_connectionString))
		{
			try
			{
				const string sql = "SELECT * FROM matches WHERE id = @MatchID;";
				var result = await connection.QueryFirstOrDefaultAsync<Match?>(sql, new { MatchID = matchID });
				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error getting match with ID {matchID}");
				throw;
			}
		}
	}

	// === POST METHODS ===
	public async Task<int> AddPlayerAsync(Player player)
	{
		using (var connection = new NpgsqlConnection(_connectionString))
		{
			try
			{
				const string sql = @"
            INSERT INTO players (player_id, player_name, total_wins, total_losses, status, is_eliminated, seeding)
            VALUES (@PlayerId, @PlayerName, @TotalWins, @TotalLosses, @Status, @IsEliminated, @Seeding)
            RETURNING id;";

				return await connection.QuerySingleAsync<int>(sql, new
				{
					player.PlayerId,
					player.PlayerName,
					player.TotalWins,
					player.TotalLosses,
					player.Status,
					player.IsEliminated,
					player.Seeding
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error adding player {player.PlayerId}");
				throw;
			}
		}
	}

	public async Task<int> UpdatePlayerAsync(Player player)
	{
		using (var connection = new NpgsqlConnection(_connectionString))
		{
			try
			{
				const string sql = @"
                UPDATE players
                SET player_name = @PlayerName, 
                    total_wins = @TotalWins, 
                    total_losses = @TotalLosses, 
                    status = @Status, 
                    is_eliminated = @IsEliminated, 
                    seeding = @Seeding
                WHERE player_id = @PlayerId;";

				return await connection.ExecuteAsync(sql, player);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error updating player {player.PlayerId}");
				throw;
			}
		}
	}

	public async Task<int> UpdateMatchAsync(Match match)
	{
		using (var connection = new NpgsqlConnection(_connectionString))
		{
			try
			{
				const string sql = @"
                UPDATE matches 
                SET 
                    winner_id = @WinnerId, 
                    time_start = @TimeStart,
                    last_updated = @LastUpdated,
                    match_type = @Type, 
                    schedule_id = @ScheduleId 
                WHERE id = @Id";

				int affectedRows = await connection.ExecuteAsync(sql, new
				{
					match.Id,
					match.WinnerId,
					match.TimeStart,
					match.LastUpdated,
					match.Type,
					match.ScheduleId
				});

				return affectedRows;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error updating match {match.Id}");
				throw;
			}
		}
	}

	public async Task<List<Schedule>> GetSchedulesAsync()
	{
		using (var connection = new NpgsqlConnection(_connectionString))
		{
			try
			{
				const string sql = "SELECT * FROM schedule;";
				var result = await connection.QueryAsync<Schedule>(sql);
				return result.ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting schedules");
				throw;
			}
		}
	}

	public async Task<int> AddScheduleAsync(Schedule schedule)
	{
		using (var connection = new NpgsqlConnection(_connectionString))
		{
			try
			{
				const string sql = @"
		INSERT INTO schedule (title, description, timestamp)
		VALUES (@Title, @Description, @Timestamp)
		RETURNING id;";

				return await connection.ExecuteAsync(sql, new
				{
					schedule.Title,
					schedule.Description,
					schedule.Timestamp
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error adding new schedule");
				throw;
			}
		}
	}

	public async Task<int> UpdateScheduleAsync(Schedule schedule)
	{
		using (var connection = new NpgsqlConnection(_connectionString))
		{
			try
			{
				const string sql = @"
		UPDATE schedule
		SET title = @Title, 
			description = @Description, 
			timestamp = @Timestamp
		WHERE id = @Id;";

				return await connection.ExecuteAsync(sql, schedule);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating schedule");
				throw;
			}
		}
	}

	public async Task<int> AddMapAsync(Map map)
	{
		using (var connection = new NpgsqlConnection(_connectionString))
		{
			try
			{
				const string sql = @"
		INSERT INTO maps (map_id, map_parameters)
		VALUES (@MapId, @MapParameters)
		RETURNING id;";

				return await connection.ExecuteAsync(sql, new
				{
					map.MapId,
					map.MapParameters
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error adding new map");
				throw;
			}
		}
	}

	public async Task<List<Map>> GetMapsAsync()
	{
		using (var connection = new NpgsqlConnection(_connectionString))
		{
			try
			{
				const string sql = "SELECT * FROM maps;";
				var result = await connection.QueryAsync<Map>(sql);
				return result.ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting maps");
				throw;
			}
		}
	}

	public async Task<Map?> GetMapAsync(long mapId)
	{
		using (var connection = new NpgsqlConnection(_connectionString))
		{
			try
			{
				const string sql = "SELECT * FROM maps WHERE map_id = @MapId;";
				var result = await connection.QueryFirstOrDefaultAsync<Map?>(sql, new { MapId = mapId });
				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error getting map with ID {mapId}");
				throw;
			}
		}
	}
}