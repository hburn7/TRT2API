using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using TRT2API.Data.Models;
using TRT2API.Data.Repositories.Interfaces;
using TRT2API.Settings;

namespace TRT2API.Data.Repositories;

public class ScheduleRepository : IScheduleRepository
{
	private readonly string _connectionString;
	private readonly ILogger<ScheduleRepository> _logger;

	public ScheduleRepository(IOptions<DatabaseSettings> dbSettings, ILogger<ScheduleRepository> logger)
	{
		_connectionString = dbSettings.Value.ConnectionString;
		_logger = logger;
	}

	public async Task<List<Schedule>> GetAllAsync()
	{
		const string sql = "SELECT * FROM schedule";

		try
		{
			using var connection = new NpgsqlConnection(_connectionString);
			return (await connection.QueryAsync<Schedule>(sql)).ToList();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error getting all schedules");
			throw;
		}
	}

	public async Task<Schedule> AddAsync(Schedule schedule)
	{
		const string sql = @"
            INSERT INTO schedule(title, description, type, image, priority, link, timestamp)
            VALUES(@Title, @Description, @Type, @Image, @Priority, @Link, @Timestamp)
            RETURNING *;";

		try
		{
			using var connection = new NpgsqlConnection(_connectionString);
			return await connection.QuerySingleAsync<Schedule>(sql, schedule);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error adding a schedule");
			throw;
		}
	}

	public async Task<Schedule> UpdateAsync(Schedule schedule)
	{
		const string sql = @"
            UPDATE schedule SET 
            title = @Title, 
            description = @Description, 
            type = @Type, 
            image = @Image, 
            priority = @Priority, 
            link = @Link, 
            timestamp = @Timestamp 
            WHERE id = @Id 
            RETURNING *;";

		try
		{
			using var connection = new NpgsqlConnection(_connectionString);
			return await connection.QuerySingleAsync<Schedule>(sql, schedule);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error updating a schedule");
			throw;
		}
	}

	public async Task DeleteAsync(int id)
	{
		const string sql = "DELETE FROM schedule WHERE id = @Id RETURNING *;";

		try
		{
			using var connection = new NpgsqlConnection(_connectionString);
			await connection.QueryAsync(sql, new { Id = id });
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error deleting a schedule");
			throw;
		}
	}

	public async Task<Schedule> GetByIdAsync(int id)
	{
		const string sql = "SELECT * FROM schedule WHERE id = @Id";
		using var connection = new NpgsqlConnection(_connectionString);
		return await connection.QueryFirstOrDefaultAsync<Schedule>(sql, new { Id = id });
	}
}