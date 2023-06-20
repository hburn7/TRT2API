using TRT2API.Data.Repositories.Interfaces;

namespace TRT2API.Data.Repositories;

public class DataWorker : IDataWorker
{
	public DataWorker(IPlayerRepository players, IMatchRepository matches, IScheduleRepository schedules, IMapRepository maps,
		IMatchMapRepository matchMaps, IMatchPlayerRepository matchPlayers)
	{
		Players = players;
		Matches = matches;
		Schedules = schedules;
		Maps = maps;
		MatchMaps = matchMaps;
		MatchPlayers = matchPlayers;
	}

	public IPlayerRepository Players { get; }
	public IMatchRepository Matches { get; }
	public IScheduleRepository Schedules { get; }
	public IMatchMapRepository MatchMaps { get; }
	public IMatchPlayerRepository MatchPlayers { get; }
	public IMapRepository Maps { get; }
}