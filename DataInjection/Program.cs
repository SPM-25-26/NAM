using DataInjection;
using DataInjection.Fetchers;
using DataInjection.Interfaces;
using DataInjection.Sync;
using DotNetEnv;
using DotnetGeminiSDK;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
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

    builder.Services.AddHostedService<DailyDataSyncWorker>();

    builder.Services.AddScoped<IFetcher, HttpFetcherService>();
    builder.Services.AddScoped<ISyncService, NewSyncService>();

    builder.Services.AddHttpClient();

    builder.Services.AddGeminiClient(config =>
    {
        config.ApiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
        config.TimeoutSeconds = 30;
        config.EnableRetry = true;
        config.MaxRetryAttempts = 3;
        config.EnableLogging = true;
    });


    var host = builder.Build();

    using (var scope = host.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        Console.WriteLine("Applying migrations...");

        // 2. Applica le migrazioni
        await dbContext.Database.MigrateAsync();

        Console.WriteLine("Migrations completed!");
    }

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