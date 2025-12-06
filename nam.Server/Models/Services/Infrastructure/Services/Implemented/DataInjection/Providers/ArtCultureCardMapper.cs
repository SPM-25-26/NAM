using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Providers
{

    public class ArtCultureCardMapper : IDtoMapper<List<ArtCultureNatureCardDto>, List<ArtCultureNatureCard>>
    {
        public List<ArtCultureNatureCard> MapToEntity(List<ArtCultureNatureCardDto> dtos)
        {
            var entities = new List<ArtCultureNatureCard>();
            foreach (var dto in dtos)
            {
                if (dto is null) continue;

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
                entities.Add(card);
            }

            return entities;
        }
    }
}
