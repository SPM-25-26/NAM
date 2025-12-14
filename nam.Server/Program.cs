using nam.Server.Endpoints.Auth;
using nam.Server.Endpoints.MunicipalityEntities;
using nam.Server.Extensions;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);


// Configure Serilog logging
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();


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
app.Run();
