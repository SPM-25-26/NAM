using Domain.Entities.MunicipalityEntities;

namespace Infrastructure.Repositories.Interfaces.MunicipalityEntities
{
    public interface IOrganizationRepository : IRepository<OrganizationCard, string>, IMunicipalityEntityRepository<OrganizationCard, OrganizationMobileDetail, string>
    {
    }
}
