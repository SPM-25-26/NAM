using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Fetchers
{
    public class ArtCultureHttpFetcher(HttpClient httpClient, ILogger logger, IConfiguration Configuration, Dictionary<string, string?> query) : AbstractHttpFetcher<ArtCultureNatureCardDto, List<ArtCultureNatureCard>>(httpClient, logger, Configuration, query)
    {
        protected override string GetEndpointUrl()
        {
            return "api/art-culture/card-list";
        }

        protected override List<ArtCultureNatureCard> MapToEntity(ArtCultureNatureCardDto dto)
        {
            var result = new List<ArtCultureNatureCard>();

            if (dto is null)
            {
                return result;
            }

            // Try to parse incoming EntityId; if invalid or empty generate a new Guid
            Guid entityId = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(dto.EntityId))
            {
                Guid.TryParse(dto.EntityId, out entityId);
            }
            if (entityId == Guid.Empty)
            {
                entityId = Guid.NewGuid();
            }

            var card = new ArtCultureNatureCard
            {
                EntityId = entityId,
                EntityName = dto.EntityName ?? string.Empty,
                ImagePath = dto.ImagePath ?? string.Empty,
                BadgeText = dto.BadgeText ?? string.Empty,
                Address = dto.Address ?? string.Empty
            };

            result.Add(card);
            return result;
        }
    }
}
