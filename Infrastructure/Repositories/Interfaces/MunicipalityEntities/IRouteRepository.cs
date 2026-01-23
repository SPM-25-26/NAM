using Domain.Entities.MunicipalityEntities;

namespace Infrastructure.Repositories.Interfaces.MunicipalityEntities
{
    public interface IRouteRepository: IRepository<RouteCard, Guid>, IMunicipalityEntityRepository<RouteCard, RouteDetail, Guid>
    {
    }
}
