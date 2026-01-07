using DataInjection.Core.Fetchers;
using DataInjection.Core.Interfaces;
using DataInjection.Qdrant;
using DataInjection.Qdrant.Data;
using DotNetEnv;
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

    builder.AddQdrantClient("vectordb");
    builder.Services.AddQdrantCollection<Guid, POIEntity>("POI-vectors");

    builder.Services.AddGoogleAIEmbeddingGenerator(
        "gemini-embedding-001",
        Environment.GetEnvironmentVariable("GEMINI_API_KEY")
    );

    builder.Services.AddHttpClient<Worker>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration.GetConnectionString("server")
            ?? throw new InvalidOperationException("Connection string 'server' not found."));
    })
    .AddStandardResilienceHandler();

    builder.Services.AddSerilog((services, lc) => lc
                .ReadFrom.Configuration(builder.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.Console());

    builder.Services.AddHostedService<Worker>();
    builder.Services.AddScoped<IFetcher, HttpFetcherService>();


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