using nam.Server.Models.Entities.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Repositories.Interfaces.MunicipalityEntities
{
    public interface IEntertainmentLeisureRepository : IRepository<EntertainmentLeisureCard, Guid>, IMunicipalityEntityRepository<EntertainmentLeisureCard, EntertainmentLeisureDetail, Guid>
    {
    }
}
