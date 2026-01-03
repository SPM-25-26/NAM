using DataInjection.DTOs;
using DataInjection.Interfaces;
using Domain.Entities.MunicipalityEntities;

namespace DataInjection.Mappers
{
    public class PublicEventCardMapper : IDtoMapper<List<PublicEventCardDto>, List<PublicEventCard>>
    {
        public List<PublicEventCard> MapToEntity(List<PublicEventCardDto> dto)
        {
            if (dto == null || dto.Count == 0)
                return [];

            return dto.Where(d => d is not null)
                .Select(dto => new PublicEventCard
                {
                    EntityId = Guid.TryParse(dto.EntityId, out var parsed) ? parsed : Guid.NewGuid(),
                    EntityName = dto.EntityName?.Trim() ?? string.Empty,
                    ImagePath = dto.ImagePath?.Trim() ?? string.Empty,
                    BadgeText = dto.BadgeText?.Trim() ?? string.Empty,
                    Address = dto.Address?.Trim() ?? string.Empty,
                    MunicipalityData = dto.MunicipalityData != null
                        ? new MunicipalityForLocalStorageSetting
                        {
                            Id = Guid.NewGuid(),
                            Name = dto.MunicipalityData.Name ?? string.Empty,
                            LogoPath = dto.MunicipalityData.LogoPath ?? string.Empty
                        }
                        : null,
                    Date = dto.Date?.Trim() ?? string.Empty
                }).ToList();
        }
    }
}
