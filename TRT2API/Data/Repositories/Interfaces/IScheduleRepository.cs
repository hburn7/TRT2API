using TRT2API.Data.Models;

namespace TRT2API.Data.Repositories.Interfaces;

public interface IScheduleRepository : IRepository<Schedule>
{
	public Task<Schedule> GetByIdAsync(int id);
}