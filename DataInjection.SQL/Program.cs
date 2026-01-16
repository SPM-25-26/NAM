using DataInjection.Core.Fetchers;
using DataInjection.Core.Interfaces;
using DataInjection.SQL;
using DataInjection.SQL.Sync;
using DotNetEnv;
using Infrastructure;
using nam.ServiceDefaults;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
Env.Load();
try
{
    var builder = Host.CreateApplicationBuilder(args);

    builder.AddServiceDefaults();

    builder.AddSqlServerDbContext<ApplicationDbContext>("db");
    builder.AddSqlServerClient("db");

    builder.Services.AddSerilog((services, lc) => lc
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console());

    builder.Services.AddHostedService<Worker>();

    builder.Services.AddScoped<IFetcher, HttpFetcherService>();
    builder.Services.AddScoped<ISyncService, NewSyncService>();

    builder.Services.AddHttpClient();

    var host = builder.Build();

    host.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}