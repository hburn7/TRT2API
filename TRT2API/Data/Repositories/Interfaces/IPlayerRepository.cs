using TRT2API.Data.Models;

namespace TRT2API.Data.Repositories.Interfaces;

public interface IPlayerRepository : IRepository<Player>
{
	public Task<Player> GetByOsuPlayerIdAsync(long osuPlayerId);
	public Task<List<Player>> GetByMatchIdAsync(int matchId);
	public Task<Player> IncrementWinsAsync(long osuPlayerId);
}