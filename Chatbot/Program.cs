using Chatbot.Controllers;
using Chatbot.Services;
using DataInjection.Qdrant.Data;
using DotNetEnv;
using Microsoft.SemanticKernel;
using nam.ServiceDefaults;
using Serilog;


var builder = WebApplication.CreateBuilder(args);
Env.Load();

builder.AddServiceDefaults();

builder.Services.AddHttpClient("entities-api", client =>
{
    // Use the name defined in the AppHost ("server")
    client.BaseAddress = new Uri("https+http://server");
}).AddServiceDiscovery();

//builder.Services.AddApplicationServices(builder.Configuration, builder.Environment);

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
    apiKey: Environment.GetEnvironmentVariable("GEMINI_API_KEY"),
    endpoint: new Uri("https://generativelanguage.googleapis.com/v1beta/openai/")
);

//builder.Services.AddSingleton<VectorStore, QdrantVectorStore>();
//builder.Services.AddSingleton<VectorStoreCollection<Guid, POIEntity>, QdrantVectorStore>();

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
app.MapChatbot();
app.Run();
