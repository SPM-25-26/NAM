using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Services.Interfaces.DataInjection;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Mappers
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