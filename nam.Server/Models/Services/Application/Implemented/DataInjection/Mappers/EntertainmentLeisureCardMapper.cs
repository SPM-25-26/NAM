using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Application.Interfaces.DataInjection;

namespace nam.Server.Models.Services.Application.Implemented.DataInjection.Mappers
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
