using Domain.Entities.MunicipalityEntities;
using Infrastructure.UnitOfWork;

namespace nam.Server.Services.Implemented.MunicipalityEntities
{
    public class RouteService(IUnitOfWork unitOfWork) : MunicipalityEntityService<RouteCard, RouteDetail>(unitOfWork.Route)
    {
    }
}
