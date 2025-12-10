using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Services.Interfaces.DataInjection;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Mappers
{
    public class EntertainmentLeisureCardMapper : IDtoMapper<List<EntertainmentLeisureCardDto>, List<EntertainmentLeisureCard>>
    {
        public List<EntertainmentLeisureCard> MapToEntity(List<EntertainmentLeisureCardDto> dto)
        {
            if (dto == null)
            {
                return new List<EntertainmentLeisureCard>();
            }

            var result = new List<EntertainmentLeisureCard>();

            foreach (var item in dto)
            {
                if (item == null)
                {
                    continue;
                }

                var entityId = Guid.TryParse(item.EntityId, out var parsedId) ? parsedId : Guid.NewGuid();
                result.Add(new EntertainmentLeisureCard
                {
                    EntityId = entityId,
                    EntityName = item.EntityName,
                    ImagePath = item.ImagePath,
                    BadgeText = item.BadgeText,
                    Address = item.Address
                });
            }

            return result;
        }
    }
}
