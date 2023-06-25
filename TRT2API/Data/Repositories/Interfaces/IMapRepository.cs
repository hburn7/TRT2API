using TRT2API.Data.Models;

namespace TRT2API.Data.Repositories.Interfaces;

public interface IMapRepository : IRepository<Map>
{
	Task<Map> GetByOsuMapIdAsync(long osuMapId);
	Task DeleteAsync(long osuMapId);
}