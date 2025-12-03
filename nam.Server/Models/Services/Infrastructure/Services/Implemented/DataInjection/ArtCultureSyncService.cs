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
        // Cache dei dettagli scaricati: accessibile sia al fetch che al map
        private readonly Dictionary<Guid, ArtCultureNatureDetail> _detailCache = new();

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

            if (dtos == null || dtos.Count == 0)
                return new List<ArtCultureNatureCardDto>();

            // Per ogni card, chiedo il dettaglio e lo salvo in cache
            foreach (var dto in dtos)
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.EntityId))
                {
                    _logger.LogWarning("Skipping detail fetch: dto or dto.EntityId is null/empty");
                    continue;
                }

                if (!Guid.TryParse(dto.EntityId, out var guidId))
                {
                    _logger.LogWarning("Skipping detail fetch: invalid GUID '{EntityId}'", dto.EntityId);
                    continue;
                }

                // build detail endpoint
                var detailPath = $"api/art-culture/detail/{Uri.EscapeDataString(dto.EntityId)}";
                var detailRelative = detailPath;
                var detailFullUri = new Uri(new Uri(baseUrl.TrimEnd('/') + "/"), detailRelative).ToString();

                try
                {
                    var detail = await client.GetFromJsonAsync<ArtCultureNatureDetail>(detailFullUri, options);
                    if (detail != null)
                    {
                        // Assicuro che l'identifier nella risorsa sia coerente col guid parsato
                        if (detail.Identifier == Guid.Empty)
                            detail.Identifier = guidId;

                        _detailCache[guidId] = detail;
                    }
                    else
                    {
                        _logger.LogWarning("Detail endpoint returned null for id {Id}", dto.EntityId);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to fetch detail for id {Id}", dto.EntityId);
                }
            }

            return dtos;
        }

        protected override List<ArtCultureNatureCard> MapToEntities(List<ArtCultureNatureCardDto> dtos)
        {
            if (dtos == null || dtos.Count == 0)
                return [];

            var entities = new List<ArtCultureNatureCard>(dtos.Count);

            foreach (var dto in dtos)
            {
                // If the ID is not valid, generate a new Guid
                var entityId = Guid.NewGuid();
                if (!string.IsNullOrWhiteSpace(dto?.EntityId) && Guid.TryParse(dto.EntityId, out var parsedId))
                    entityId = parsedId;

                // Try to get the previously fetched detail from cache
                _detailCache.Remove(entityId, out var detailFromCache);

                var entity = new ArtCultureNatureCard
                {
                    EntityId = entityId,
                    EntityName = dto?.EntityName?.Trim() ?? string.Empty,
                    ImagePath = dto?.ImagePath?.Trim() ?? string.Empty,
                    BadgeText = dto?.BadgeText?.Trim() ?? string.Empty,
                    Address = dto?.Address?.Trim() ?? string.Empty,
                    Detail = detailFromCache
                };
                entities.Add(entity);
            }

            return entities;
        }
    }
}
