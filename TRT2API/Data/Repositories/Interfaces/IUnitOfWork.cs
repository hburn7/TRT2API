namespace TRT2API.Data.Repositories.Interfaces;

public interface IDataWorker
{
	IPlayerRepository Players { get; }
	IMapRepository Maps { get; }
	IMatchRepository Matches { get; }
	IScheduleRepository Schedules { get; }
	IMatchMapRepository MatchMaps { get; }
	IMatchPlayerRepository MatchPlayers { get; }
}