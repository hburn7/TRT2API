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

        public async Task<List<Player>> GetPlayersAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string sql = "SELECT * FROM players;";
                var result = await connection.QueryAsync<Player>(sql);
                return result.ToList();
            }
        }

        public async Task<List<Player>> GetPlayersAsync(int matchID)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string sql = @"SELECT p.* 
                                    FROM players p 
                                    JOIN matches m ON p.playerid = ANY(m.playerids) 
                                    WHERE m.matchid = @MatchID;";
                var result = await connection.QueryAsync<Player>(sql, new { MatchID = matchID });
                return result.ToList();
            }
        }

        public async Task<List<Match>> GetMatchesAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string sql = "SELECT * FROM matches;";
                var result = await connection.QueryAsync<Match>(sql);
                return result.ToList();
            }
        }

        public async Task<List<Match>> GetMatchesAsync(long playerID)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string sql = "SELECT *, array_to_string(playerids, ',') AS player_ids_string FROM matches WHERE playerids @> ARRAY[@PlayerID];";

                var matches = await connection.QueryAsync<Match, string, Match>(sql,
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

        public async Task<Player?> GetPlayerAsync(long playerID)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string sql = "SELECT * FROM players WHERE playerid = @PlayerID;";
                var result = await connection.QueryFirstOrDefaultAsync<Player?>(sql, new { PlayerID = playerID });
                return result;
            }
        }

        public async Task<Match?> GetMatchAsync(long matchID)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                const string sql = "SELECT * FROM matches WHERE matchid = @MatchID;";
                var result = await connection.QueryFirstOrDefaultAsync<Match?>(sql, new { MatchID = matchID });
                return result;
            }
        }
	
	// === POST METHODS ===
    public async Task<int> AddPlayerAsync(Player player)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            const string sql = @"
            INSERT INTO players (playerid, playername, totalwins, totallosses, status, iseliminated)
            VALUES (@PlayerID, @PlayerName, @TotalWins, @TotalLosses, @Status, @IsEliminated)
            RETURNING id;";

            return await connection.ExecuteAsync(sql, new 
            {
                player.PlayerID,
                player.PlayerName,
                player.TotalWins,
                player.TotalLosses,
                player.Status,
                player.IsEliminated
            });
        }
    }

    public async Task<int> UpdatePlayerAsync(Player player)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            const string sql = @"
            UPDATE players
            SET PlayerName = @PlayerName, 
                TotalWins = @TotalWins, 
                TotalLosses = @TotalLosses, 
                Status = @Status, 
                IsEliminated = @IsEliminated 
            WHERE PlayerID = @PlayerID;";

            return await connection.ExecuteAsync(sql, player);
        }
    }

    public async Task<int> UpdateMatchAsync(Match match)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            const string sql = @"
            UPDATE matches 
            SET PlayerIDs = @PlayerIDs::bigint[], 
                WinnerID = @WinnerID, 
                TimeStart = @TimeStart, 
                LastUpdated = @LastUpdated 
            WHERE MatchID = @MatchID;";
            
            return await connection.ExecuteAsync(sql, new
            {
                PlayerIDs = match.PlayerIDs != null ? string.Join(",", match.PlayerIDs) : null,
                match.WinnerID,
                match.TimeStart,
                match.LastUpdated,
                match.MatchID
            });
        }
    }
}