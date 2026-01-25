using Datainjection.Qdrant.Collectors;
using Datainjection.Qdrant.Sync;
using DataInjection.Core.Interfaces;
using DataInjection.Qdrant;
using DataInjection.Qdrant.Data;
using DotNetEnv;
using Infrastructure;
using Infrastructure.UnitOfWork;
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
    builder.AddSqlServerDbContext<ApplicationDbContext>("db");
    builder.AddSqlServerClient("db");

    builder.Services.AddGoogleAIEmbeddingGenerator(
        "gemini-embedding-001",
        Environment.GetEnvironmentVariable("GEMINI_API_KEY")
    );

    builder.Services.AddSerilog((services, lc) => lc
                .ReadFrom.Configuration(builder.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.Console());

    builder.Services.AddHostedService<Worker>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<QdrantEntitySync>();

    //builder.Services.AddScoped<IEntityCollector<POIEntity>, ArtCultureQdrantCollector>();
    //builder.Services.AddScoped<IEntityCollector<POIEntity>, ArticleQdrantCollector>();
    //builder.Services.AddScoped<IEntityCollector<POIEntity>, EntertainmentLeisureCardQdrantCollector>();
    //builder.Services.AddScoped<IEntityCollector<POIEntity>, NatureQdrantCollector>();
    //builder.Services.AddScoped<IEntityCollector<POIEntity>, OrganizationQdrantCollector>();
    //builder.Services.AddScoped<IEntityCollector<POIEntity>, PublicEventQdrantCollector>();
    //builder.Services.AddScoped<IEntityCollector<POIEntity>, EatAndDrinkQdrantCollector>();
    //builder.Services.AddScoped<IEntityCollector<POIEntity>, SleepQdrantCollector>();
    builder.Services.AddScoped<IEntityCollector<POIEntity>, ShoppingQdrantCollector>();
    //builder.Services.AddScoped<IEntityCollector<POIEntity>, ServiceQdrantCollector>();
    //builder.Services.AddScoped<IEntityCollector<POIEntity>, RouteQdrantCollector>();


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