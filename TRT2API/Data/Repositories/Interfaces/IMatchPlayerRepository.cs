using TRT2API.Data.Models;

namespace TRT2API.Data.Repositories.Interfaces;

public interface IMatchPlayerRepository : IRepository<MatchPlayer>
{
		Task<List<MatchPlayer>> GetByMatchIdAsync(long matchId);
		Task IncrementScoreAsync(long matchId, long playerId);
}