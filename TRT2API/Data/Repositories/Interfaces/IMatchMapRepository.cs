using TRT2API.Data.Models;

namespace TRT2API.Data.Repositories.Interfaces;

public interface IMatchMapRepository : IRepository<MatchMap>
{
	Task<List<MatchMap>> GetByMatchIdAsync(int matchId);
}