using TRT2API.Data.Repositories.Interfaces;

namespace TRT2API.Data.Repositories;

public class DataWorker : IDataWorker
{
	public DataWorker(IPlayerRepository players, IMatchRepository matches, IScheduleRepository schedules, IMapRepository maps)
	{
		Players = players;
		Matches = matches;
		Schedules = schedules;
		Maps = maps;
	}

	public IPlayerRepository Players { get; }
	public IMatchRepository Matches { get; }
	public IScheduleRepository Schedules { get; }
	public IMapRepository Maps { get; }
}