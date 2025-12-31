using DataInjection;
using DataInjection.Fetchers;
using DataInjection.Interfaces;
using DataInjection.Qdrant;
using DataInjection.Qdrant.Data;
using DataInjection.Sync;
using DotNetEnv;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel.Connectors.Google;
using Polly;
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

    builder.AddQdrantClient("vectordb");
    builder.Services.AddQdrantCollection<Guid, POIEntity>("POI-vectors");

    builder.Services.AddHttpClient<GoogleAIEmbeddingGenerator>(client => { })
    .AddStandardResilienceHandler(options =>
    {
        // 1. RETRY STRATEGY
        // Change from "Fixed" to "Exponential" to back off when 429 occurs
        options.Retry.BackoffType = DelayBackoffType.Exponential;
        options.Retry.MaxRetryAttempts = 50;
        options.Retry.Delay = TimeSpan.FromSeconds(60); // Initial delay
        options.Retry.UseJitter = true; // Randomizes delay to prevent "thundering herd"

        // 2. CIRCUIT BREAKER
        // Make it stay open longer once it trips to give the API time to recover
        options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(30);
        options.CircuitBreaker.FailureRatio = 0.5; // Trip if 50% of calls fail in the window
        options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(60);

        // 3. ATTEMPT TIMEOUT
        // Ensure individual requests don't hang too long
        options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(10);
    });

    // A questo punto, la registrazione standard userà automaticamente il client configurato sopra
    builder.Services.AddGoogleAIEmbeddingGenerator(
        "gemini-embedding-001",
        Environment.GetEnvironmentVariable("GEMINI_API_KEY")
    );

    //builder.Services.AddGoogleAIEmbeddingGenerator("gemini-embedding-001", Environment.GetEnvironmentVariable("GEMINI_API_KEY"))
    //builder.Services.AddGoogleAIGeminiChatCompletion("gemini-2.0-flash-lite", Environment.GetEnvironmentVariable("GEMINI_API_KEY"));

    builder.Services.AddSerilog((services, lc) => lc
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console());

    builder.Services.AddHostedService<DailyDataSyncWorker>();
    builder.Services.AddHostedService<QdrantDataSyncWorker>();

    builder.Services.AddScoped<IFetcher, HttpFetcherService>();
    builder.Services.AddScoped<ISyncService, NewSyncService>();

    builder.Services.AddHttpClient();
    builder.Services.AddHttpClient<HttpFetcherService>(client => { })
    .AddStandardResilienceHandler(options =>
    {
        // 1. RETRY STRATEGY
        // Change from "Fixed" to "Exponential" to back off when 429 occurs
        options.Retry.BackoffType = DelayBackoffType.Exponential;
        options.Retry.MaxRetryAttempts = 3;
        options.Retry.Delay = TimeSpan.FromSeconds(2); // Initial delay
        options.Retry.UseJitter = true; // Randomizes delay to prevent "thundering herd"

        // 2. CIRCUIT BREAKER
        // Make it stay open longer once it trips to give the API time to recover
        options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(30);
        options.CircuitBreaker.FailureRatio = 0.5; // Trip if 50% of calls fail in the window
        options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(60);

        // 3. ATTEMPT TIMEOUT
        // Ensure individual requests don't hang too long
        options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(10);
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