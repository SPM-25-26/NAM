using DataInjection;
using DataInjection.Fetchers;
using DataInjection.Interfaces;
using DataInjection.Sync;
using Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.AddSqlServerDbContext<ApplicationDbContext>("db");
builder.AddSqlServerClient("db");

builder.Services.AddHostedService<DailyDataSyncWorker>();

builder.Services.AddTransient<IFetcher, HttpFetcherService>();
builder.Services.AddTransient<ISyncService, NewSyncService>();

builder.Services.AddHttpClient();

var host = builder.Build();
host.Run();
