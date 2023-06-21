using TRT2API.Data.Models;

namespace TRT2API.Data.Repositories.Interfaces;

public interface IRoundRepository
{
	public Task<List<Round>?> GetAllAsync();
	public Task<Round?> GetAsync(string name);
}