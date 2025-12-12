using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Application.Interfaces.DataInjection;

namespace nam.Server.Models.Services.Application.Implemented.DataInjection.Mappers
{
    public class OrganizationCardMapper : IDtoMapper<List<OrganizationCardDto>, List<OrganizationCard>>
    {

        public List<OrganizationCard> MapToEntity(List<OrganizationCardDto> dtos)
        {
            if (dtos == null || dtos.Count == 0)
                return new List<OrganizationCard>();

            return dtos
                .Where(d => d is not null)
                .Select(d =>
                {
                    return new OrganizationCard
                    {
                        TaxCode = d.EntityId,
                        EntityName = string.IsNullOrWhiteSpace(d.EntityName) ? null : d.EntityName!.Trim(),
                        ImagePath = string.IsNullOrWhiteSpace(d.ImagePath) ? null : d.ImagePath!.Trim(),
                        BadgeText = string.IsNullOrWhiteSpace(d.BadgeText) ? null : d.BadgeText!.Trim(),
                        Address = string.IsNullOrWhiteSpace(d.Address) ? null : d.Address!.Trim(),
                        Detail = null
                    };
                })
                .ToList();
        }
    }
}
