using Domain.Entities.MunicipalityEntities;

namespace Infrastructure.Repositories.Interfaces.MunicipalityEntities
{
    public interface IPublicEventRepository : IRepository<PublicEventCard, Guid>, IMunicipalityEntityRepository<PublicEventCard, PublicEventMobileDetail, Guid>
    {
    }
}
