using Domain.Entities.MunicipalityEntities;
using Infrastructure.UnitOfWork;

namespace nam.Server.Services.Implemented.MunicipalityEntities
{
    public class ServiceService(IUnitOfWork unitOfWork) : MunicipalityEntityService<ServiceCard, ServiceDetail>(unitOfWork.Service)
    {
    }
}
