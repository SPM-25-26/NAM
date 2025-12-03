using Microsoft.AspNetCore.WebUtilities;
using nam.Server.Data;
using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;
using System.Text.Json;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection
{
    public class ArtCultureSyncService(ApplicationDbContext dbContext, ILogger logger, IConfiguration Configuration, string municipality, IHttpClientFactory httpClientFactory)
        : BaseSyncService<ArtCultureNatureCardDto, ArtCultureNatureCard>(dbContext, logger, Configuration, municipality)
    {
        protected override async Task<List<ArtCultureNatureCardDto>> FetchDataFromApiAsync()
        {
            var baseUrl = Configuration["DataInjectionApi"];
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new InvalidOperationException("Configuration key 'DataInjectionApi' is not set.");

            if (string.IsNullOrWhiteSpace(municipality))
                throw new InvalidOperationException("Configuration key 'DataInjectionMunicipality' is not set. The municipality string is required by the API.");

            var client = httpClientFactory.CreateClient();

            var endpointPath = "api/art-culture/card-list";
            var query = new Dictionary<string, string?> { ["municipality"] = municipality };
            var relativeWithQuery = QueryHelpers.AddQueryString(endpointPath, query);

            var fullUri = new Uri(new Uri(baseUrl.TrimEnd('/') + "/"), relativeWithQuery).ToString();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var dtos = await client.GetFromJsonAsync<List<ArtCultureNatureCardDto>>(fullUri, options);

            return dtos ?? [];
        }

        protected override List<ArtCultureNatureCard> MapToEntities(List<ArtCultureNatureCardDto> dtos)
        {
            if (dtos == null || dtos.Count == 0)
                return new List<ArtCultureNatureCard>();

            var entities = new List<ArtCultureNatureCard>(dtos.Count);

            foreach (var dto in dtos)
            {
                // If the ID is not valid, generate a new Guid
                var entityId = Guid.NewGuid();
                if (!string.IsNullOrWhiteSpace(dto?.EntityId) && Guid.TryParse(dto.EntityId, out var parsedId))
                    entityId = parsedId;

                var entity = new ArtCultureNatureCard
                {
                    EntityId = entityId,
                    EntityName = dto?.EntityName?.Trim() ?? string.Empty,
                    ImagePath = dto?.ImagePath?.Trim() ?? string.Empty,
                    BadgeText = dto?.BadgeText?.Trim() ?? string.Empty,
                    Address = dto?.Address?.Trim() ?? string.Empty,
                    Detail = null
                };
                entities.Add(entity);
            }

            return entities;
        }
    }
}
