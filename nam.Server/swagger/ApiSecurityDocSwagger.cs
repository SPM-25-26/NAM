using Microsoft.OpenApi.Models;

namespace nam.Server.swagger;

public static class ApiSecurityDocSwagger
{
    public const string IdTokenSecurity = "Bearer"; // costante statica

    public static readonly List<OpenApiSecurityRequirement> BearerRequirement =
        new()
        {
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = IdTokenSecurity
                        }
                    },
                    Array.Empty<string>()
                }
            }
        };
}
