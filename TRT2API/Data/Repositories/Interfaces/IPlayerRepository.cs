using TRT2API.Data.Models;

namespace TRT2API.Data.Repositories.Interfaces;

public interface IPlayerRepository : IRepository<Player>
{
	public Task<Player> GetByOsuPlayerIdAsync(long playerId);
	public Task<List<Player>> GetByMatchIdAsync(long matchId);
	public Task<Player> IncrementWinsAsync(long osuPlayerId);
}