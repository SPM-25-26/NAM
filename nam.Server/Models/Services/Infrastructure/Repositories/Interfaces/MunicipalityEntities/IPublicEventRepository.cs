using nam.Server.Models.Entities.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Repositories.Interfaces.MunicipalityEntities
{
    public interface IPublicEventRepository : IRepository<PublicEventCard, Guid>, IMunicipalityEntityRepository<PublicEventCard, PublicEventMobileDetail, Guid>
    {
    }
}
