using DataInjection.Core.Interfaces;
using DataInjection.SQL.DTOs;
using Domain.Entities.MunicipalityEntities;

namespace DataInjection.SQL.Mappers
{
    public class SleepCardMapper : IDtoMapper<List<SleepCardDto>, List<SleepCard>>
    {
        public List<SleepCard> MapToEntity(List<SleepCardDto> dtos)
        {
            if (dtos == null || dtos.Count == 0)
                return [];

            return dtos.Where(dto => dto is not null)
                .Select(dto => new SleepCard
                {
                    EntityId = Guid.TryParse(dto.EntityId, out Guid entityId) ? entityId : Guid.NewGuid(),
                    EntityName = dto.EntityName ?? string.Empty,
                    ImagePath = dto.ImagePath ?? string.Empty,
                    BadgeText = dto.BadgeText ?? string.Empty,
                    Address = dto.Address ?? string.Empty
                }).ToList();
        }
    }
}
