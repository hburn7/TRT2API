using TRT2API.Data.Models;

namespace TRT2API.Data.Repositories.Interfaces;

public interface IMapRepository : IRepository<Map>
{
	Task<Map> GetByMapIdAsync(long mapId);
}