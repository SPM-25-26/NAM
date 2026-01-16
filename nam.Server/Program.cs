using DotNetEnv;
using Infrastructure;
using nam.Server.Endpoints;
using nam.Server.Endpoints.Auth;
using nam.Server.Endpoints.MunicipalityEntities;
using nam.Server.Extensions;
using nam.ServiceDefaults;
using Serilog;


var builder = WebApplication.CreateBuilder(args);
Env.Load();

builder.AddServiceDefaults();

builder.AddSqlServerDbContext<ApplicationDbContext>("db");
builder.AddSqlServerClient("db");


builder.Services.AddApplicationServices(builder.Configuration, builder.Environment);



// Configure Serilog logging
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

builder.AddQdrantClient("vectordb");
builder.Services.AddGoogleAIEmbeddingGenerator(
                "gemini-embedding-001",
                Environment.GetEnvironmentVariable("GEMINI_API_KEY")
            );

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        {
            c.DocumentTitle = "NAM API Docs";
        });
}


app.UseHttpsRedirection();
//app.UseStaticFiles();

app.UseRouting();

// New policy
app.UseCors("FrontendWithCredentials");


// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapAuth();
app.MapArtCulture();
app.MapImages();
app.MapPublicEvent();
app.MapArticle();
app.MapNature();
app.MapMunicipalityCard();
app.MapOrganization();
app.MapEntertainmentLeisure();
app.MapQuestionaire();
app.ReccomandationMap();
app.Run();
