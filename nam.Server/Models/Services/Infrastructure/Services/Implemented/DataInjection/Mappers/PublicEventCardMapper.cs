using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Services.Interfaces.DataInjection;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Mappers
{
    public class PublicEventCardMapper : IDtoMapper<List<PublicEventCardDto>, List<PublicEventCard>>
    {
        public List<PublicEventCard> MapToEntity(List<PublicEventCardDto> dto)
        {
            if (dto == null || dto.Count == 0)
            {
                return [];
            }

            var result = new List<PublicEventCard>(dto.Count);

            foreach (var item in dto)
            {
                var entityId = Guid.TryParse(item?.EntityId, out var parsed) ? parsed : Guid.NewGuid();

                MunicipalityForLocalStorageSetting? municipality = null;
                if (item?.MunicipalityData != null)
                {
                    municipality = new MunicipalityForLocalStorageSetting
                    {
                        Id = Guid.NewGuid(),
                        Name = item.MunicipalityData.Name ?? string.Empty,
                        LogoPath = item.MunicipalityData.LogoPath ?? string.Empty
                    };
                }

                var card = new PublicEventCard
                {
                    EntityId = entityId,
                    EntityName = item?.EntityName?.Trim() ?? string.Empty,
                    ImagePath = item?.ImagePath?.Trim() ?? string.Empty,
                    BadgeText = item?.BadgeText?.Trim() ?? string.Empty,
                    Address = item?.Address?.Trim() ?? string.Empty,
                    MunicipalityData = municipality,
                    MunicipalityDataId = municipality?.Id,
                    Date = item?.Date?.Trim() ?? string.Empty
                };

                result.Add(card);
            }

            return result;
        }
    }
}
