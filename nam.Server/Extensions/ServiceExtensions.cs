using DataInjection.Qdrant.Data;
using Domain.Entities.MunicipalityEntities;
using FluentValidation;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using nam.Server.Options;
using nam.Server.Services.Implemented;
using nam.Server.Services.Implemented.Auth;
using nam.Server.Services.Implemented.MunicipalityEntities;
using nam.Server.Services.Implemented.RecSys;
using nam.Server.Services.Interfaces;
using nam.Server.Services.Interfaces.Auth;
using nam.Server.Services.Interfaces.MunicipalityEntities;
using nam.Server.Services.Interfaces.RecSys;
using nam.Server.swagger;
using Qdrant.Client;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace nam.Server.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {

            //// Retrieve and configure the database connection string
            //string connectionString = configuration.GetConnectionString("DefaultConnection")
            //    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            //// Register the DbContext with SQL Server provider
            //services.AddDbContext<ApplicationDbContext>(options =>
            // options.UseSqlServer(
            //     connectionString,
            //     o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
            // ));


            // Retrieve the JWT secret key from configuration
            var key = configuration["Jwt:Secret"]
                ?? throw new InvalidOperationException("JWT secret not configured");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                        OnMessageReceived = context =>
                        {
                            // Try to take the token from the "AuthToken" cookie
                            var tokenFromCookie = context.Request.Cookies["AuthToken"];


                            if (!string.IsNullOrEmpty(tokenFromCookie))
                            {
                                context.Token = tokenFromCookie;
                            }

                            // Without cookie, it will continue to use (if any) Authorization: Bearer ...
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = async context =>
                        {
                            var tokenService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();

                            var jti = context.Principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

                            if (!string.IsNullOrEmpty(jti))
                            {

                                var isRevoked = await tokenService.IsTokenRevokedAsync(jti);

                                if (isRevoked)
                                {
                                    // Block the request: this token is blacklisted
                                    context.Fail("Token revoked");
                                }
                            }

                            var userEmail = context.Principal?.FindFirst(ClaimTypes.Email)?.Value
                            ?? context.Principal?.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
                            if (userEmail == null || !await tokenService.ValidateToken(userEmail))
                            {
                                context.Fail("User not present or token invalid");
                            }
                        }
                    };
                });

            // Enable authorization services
            /*services.AddAuthorization(options =>
            {
                // All authorized in dev mode
                if (environment.IsDevelopment())
                {
                    options.DefaultPolicy = new AuthorizationPolicyBuilder()
                        .RequireAssertion(_ => true) // Allow all requests
                        .Build();
                    options.FallbackPolicy = options.DefaultPolicy;
                }
                // Otherwise (in Prod), use the standard logic (requires authenticated user)
            });*/

            // Add services to the container.
            services.AddRazorPages();
            services.AddValidatorsFromAssemblyContaining<Program>();


            // Register Unit of Work pattern
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register application services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenGeneration, TokenGeneration>();
            services.AddScoped<IEmailService, LocalEmailService>();
            services.AddScoped<ICodeService, RandomCodeService>();
            // Register the background service for cleaning up revoked tokens
            services.AddHostedService<RevokedTokensCleanupService>();

            // Bind JWT configuration section to strongly-typed options
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));


            services.AddScoped<IUserService, UserService>();

            services.AddCors(options =>
            {
                // Policy "opened" for scenarios with credentials (if you really need it)
                options.AddPolicy("AllowAll", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });

                // Policy for the React frontend with cookies
                options.AddPolicy("FrontendWithCredentials", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:5173") // FE React
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();                  // for credentials: "include"
                });
            });

            // Register Swagger/OpenAPI
            services.AddEndpointsApiExplorer();

            // Configure Swagger to support JWT authentication
            services.AddSwaggerGen(options =>
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
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            // Register HttpClient for data injection services
            services.AddHttpClient();

            // Municipality entities services
            services.AddScoped<IMunicipalityEntityService<ArtCultureNatureCard, ArtCultureNatureDetail>, ArtCultureService>();

            services.AddScoped<IMunicipalityEntityService<PublicEventCard, PublicEventMobileDetail>, PublicEventService>();

            services.AddScoped<IMunicipalityEntityService<ArticleCard, ArticleDetail>, ArticleService>();

            services.AddScoped<IMunicipalityEntityService<Nature, ArtCultureNatureDetail>, NatureService>();

            services.AddScoped<IMunicipalityEntityService<MunicipalityCard, MunicipalityHomeInfo>, MunicipalityCardService>();

            services.AddScoped<IMunicipalityEntityService<OrganizationCard, OrganizationMobileDetail>, OrganizationService>();

            services.AddScoped<IMunicipalityEntityService<EntertainmentLeisureCard, EntertainmentLeisureDetail>, EntertainmentLeisureService>();

            services.AddScoped<IMunicipalityEntityService<RouteCard, RouteDetail>, RouteService>();

            services.AddScoped<IMunicipalityEntityService<ServiceCard, ServiceDetail>, ServiceService>();

            services.AddScoped<IMunicipalityEntityService<ShoppingCard, ShoppingCardDetail>, ShoppingService>();

            services.AddScoped<IMunicipalityEntityService<SleepCard, SleepCardDetail>, SleepService>();

            services.AddScoped<IMunicipalityEntityService<EatAndDrinkCard, EatAndDrinkDetail>, EatAndDrinkService>();

            services.AddScoped<IRanker, SimpleRanker>();
            services.AddScoped<IScorer, WeightedScorer>();
            services.AddScoped<IRecSysService, RecsysService>();

            // Ritorna services per permettere il "chaining" (concatenazione)
            return services;
        }
    }
}
