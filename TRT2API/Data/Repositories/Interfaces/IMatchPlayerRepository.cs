using TRT2API.Data.Models;

namespace TRT2API.Data.Repositories.Interfaces;

public interface IMatchPlayerRepository : IRepository<MatchPlayer>
{
		Task<List<MatchPlayer>> GetByMatchIdAsync(int matchId);
		Task<MatchPlayer?> GetAsync(int id);
		Task DeleteAsync(int id);
}