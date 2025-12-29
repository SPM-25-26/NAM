using DataInjection.DTOs;
using DataInjection.Interfaces;
using Domain.Entities.MunicipalityEntities;

namespace DataInjection.Mappers
{
    public class MunicipalityCardMapper : IDtoMapper<List<MunicipalityCardDto>, List<MunicipalityCard>>
    {
        public List<MunicipalityCard> MapToEntity(List<MunicipalityCardDto> dto)
        {
            if (dto == null || dto.Count == 0)
                return [];

            return dto
                .Where(d => d is not null)
                .Select(d =>
                {
                    return new MunicipalityCard
                    {
                        LegalName = string.IsNullOrWhiteSpace(d.LegalName) ? null : d.LegalName!.Trim(),
                        ImagePath = string.IsNullOrWhiteSpace(d.ImagePath) ? null : d.ImagePath!.Trim(),
                        Detail = null
                    };
                })
                .ToList();
        }
    }
}