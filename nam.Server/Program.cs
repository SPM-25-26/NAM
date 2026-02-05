using DataInjection.Qdrant.Data;
using DotNetEnv;
using Infrastructure;
using Microsoft.SemanticKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using nam.Server.Endpoints;
using nam.Server.Endpoints.Auth;
using nam.Server.Endpoints.Chatbot;
using nam.Server.Endpoints.MunicipalityEntities;
using nam.Server.Extensions;
using nam.Server.Services.Implemented.Chatbot;
using nam.Server.Services.Interfaces.Chatbot;
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
builder.Services.AddQdrantCollection<Guid, POIEntity>("POI-vectors");

builder.Services.AddGoogleAIEmbeddingGenerator(
    modelId: "gemini-embedding-001",
    apiKey: Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? "no-key"
);

// Chatbot config
builder.Services.AddOpenAIChatCompletion(
    modelId: "gemini-2.5-flash-lite",
    apiKey: Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? "no-key",
    endpoint: new Uri("https://generativelanguage.googleapis.com/v1beta/openai/")
);
builder.Services.AddScoped<IChatbotService, ChatbotService>();

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
app.MapRoute();
app.MapService();
app.MapShopping();
app.MapSleep();
app.MapEatAndDrink();
app.MapMapData();
app.MapQuestionaire();
app.ReccomandationMap();
app.MapChatbot();
app.Run();

public partial class Program
{
}
