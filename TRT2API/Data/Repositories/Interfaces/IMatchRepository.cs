﻿using TRT2API.Data.Models;

namespace TRT2API.Data.Repositories.Interfaces;

public interface IMatchRepository : IRepository<Match>
{
	public Task<Match> GetByOsuMatchIdAsync(long matchId);
	// public Task<List<Player>> GetPlayersForMatchIdAsync(long matchId);
	public Task<Match?> GetAsync(int id);
	public Task<List<Match>> GetByPlayerIdAsync(int playerId);
}