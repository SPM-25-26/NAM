using nam.Server.Models.Entities.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Repositories.Interfaces.MunicipalityEntities
{
    public interface IOrganizationRepository : IRepository<OrganizationCard, string>, IMunicipalityEntityRepository<OrganizationCard, OrganizationMobileDetail, string>
    {
    }
}
