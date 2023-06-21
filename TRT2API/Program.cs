using TRT2API.Data.Repositories;
using TRT2API.Data.Repositories.Interfaces;
using TRT2API.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScoped<IMapRepository, MapRepository>();
builder.Services.AddScoped<IMatchMapRepository, MatchMapRepository>();
builder.Services.AddScoped<IMatchPlayerRepository, MatchPlayerRepository>();
builder.Services.AddScoped<IRoundRepository, RoundRepository>();

builder.Services.AddScoped<IDataWorker, DataWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();