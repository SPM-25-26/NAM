using Domain.Entities.MunicipalityEntities;

namespace Infrastructure.Repositories.Interfaces.MunicipalityEntities
{
    public interface IEntertainmentLeisureRepository : IRepository<EntertainmentLeisureCard, Guid>, IMunicipalityEntityRepository<EntertainmentLeisureCard, EntertainmentLeisureDetail, Guid>
    {
    }
}
