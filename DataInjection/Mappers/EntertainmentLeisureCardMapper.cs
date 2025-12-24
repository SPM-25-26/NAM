using DataInjection.Interfaces;
using Domain.DTOs.MunicipalityInjection;
using Domain.Entities.MunicipalityEntities;

namespace DataInjection.Mappers
{
    public class EntertainmentLeisureCardMapper : IDtoMapper<List<EntertainmentLeisureCardDto>, List<EntertainmentLeisureCard>>
    {
        public List<EntertainmentLeisureCard> MapToEntity(List<EntertainmentLeisureCardDto> dtos)
        {
            if (dtos == null || dtos.Count == 0)
                return [];

            return dtos.Where(dto => dto is not null)
                .Select(dto => new EntertainmentLeisureCard
                {
                    EntityId = Guid.TryParse(dto.EntityId, out var parsedId) ? parsedId : Guid.NewGuid(),
                    EntityName = dto.EntityName,
                    ImagePath = dto.ImagePath,
                    BadgeText = dto.BadgeText,
                    Address = dto.Address
                }).ToList();
        }
    }
}
