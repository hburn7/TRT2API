namespace TRT2API.Data.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
	Task<List<T>> GetAllAsync();
	Task<T> AddAsync(T entity);
	Task<T> UpdateAsync(T entity);
	Task<T> DeleteAsync(T entity);
}