using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using nam.Server.Models.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace nam.Server.Models.swagger
{
    public class SecurityRequirementsOperationFilter : IOperationFilter

    {

        public void Apply(OpenApiOperation operation, OperationFilterContext context)

        {

            // 1. Search for "RequireAuthorization" (IAuthorizeData) in the endpoint metadata

            var hasAuthorize = context.ApiDescription.ActionDescriptor.EndpointMetadata

                .Any(meta => meta is IAuthorizeData);



            // 2. Search for "AllowAnonymous" (to exclude login if necessary)

            var allowAnonymous = context.ApiDescription.ActionDescriptor.EndpointMetadata

                .Any(meta => meta is IAllowAnonymous);



            // 3. If authorization is required and it's not anonymous -> Add the lock

            if (hasAuthorize && !allowAnonymous)

            {

                operation.Security = new List<OpenApiSecurityRequirement>

            {

                new OpenApiSecurityRequirement

                {

                    {

                        new OpenApiSecurityScheme

                        {

                            Reference = new OpenApiReference

                            {

                                Type = ReferenceType.SecurityScheme,

                                // IMPORTANT: Must match the ID used in ServiceExtensions.cs

                                Id = ApiSecurityDocSwagger.IdTokenSecurity

                            }

                        },

                        Array.Empty<string>()

                    }

                }

            };

            }

        }

    }
}
