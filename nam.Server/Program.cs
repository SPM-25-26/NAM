using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using nam.Server.Data;
using nam.Server.Endpoints;
using nam.Server.Models.Options;
using nam.Server.Models.Services.Infrastructure;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using nam.Server.Models.Services.Implementations;

using System.Text;
using nam.Server.Models.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Retrieve and configure the database connection string
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Register the DbContext with SQL Server provider
builder.Services.AddDbContext<ApplicationDbContext>(options =>
 options.UseSqlServer(
     connectionString
 ));
// Retrieve the JWT secret key from configuration
var key = builder.Configuration["Jwt:Secret"]
    ?? throw new InvalidOperationException("JWT secret not configured");

// Configure JWT Bearer authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)) // Secret key
        };
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();

                var jti = context.Principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

                if (!string.IsNullOrEmpty(jti))
                {

                    var isRevoked = await tokenService.IsTokenRevokedAsync(jti);

                    if (isRevoked)
                    {
                        // Blocco la richiesta: questo token è in blacklist
                        context.Fail("Token revoked");
                    }
                }
            }
        };
    });

// Enable authorization services
builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Configure Serilog logging
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

// Register Unit of Work pattern
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register application services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, LocalEmailService>();
builder.Services.AddScoped<ICodeService, RandomCodeService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
// Register the background service for cleaning up revoked tokens
builder.Services.AddHostedService<RevokedTokensCleanupService>();

// Bind JWT configuration section to strongly-typed options
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// Register Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger to support JWT authentication
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "NAM API",
        Version = "v1",
        Description = "API for NAM project",
    });
    options.AddSecurityDefinition(ApiSecurityDocSwagger.IdTokenSecurity, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token in the format: Bearer {your token}"
    });
});

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
app.UseStaticFiles();

app.UseRouting();
app.UseCors("AllowAll");


// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapAuth();
app.Run();