using DataInjection.DTOs;
using DataInjection.Interfaces;
using Domain.Entities.MunicipalityEntities;

namespace DataInjection.Mappers
{
    public class NatureMapper : IDtoMapper<List<ArtCultureNatureCardDto>, List<Nature>>
    {
        public List<Nature> MapToEntity(List<ArtCultureNatureCardDto> dtos)
        {
            var entities = new List<Nature>();
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

                var card = new Nature
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