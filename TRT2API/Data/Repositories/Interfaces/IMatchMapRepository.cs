using TRT2API.Data.Models;

namespace TRT2API.Data.Repositories.Interfaces;

public interface IMatchMapRepository : IRepository<MatchMap>
{
	public Task<MatchMap> GetAsync(int id);
	Task<List<MatchMap>> GetByMatchIdAsync(int matchId);
	Task DeleteAsync(int id);
}