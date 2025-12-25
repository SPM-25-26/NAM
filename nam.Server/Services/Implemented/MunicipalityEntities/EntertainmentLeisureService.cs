using Domain.Entities.MunicipalityEntities;
using Infrastructure.UnitOfWork;

namespace nam.Server.Services.Implemented.MunicipalityEntities
{
    public class EntertainmentLeisureService(IUnitOfWork unitOfWork) : MunicipalityEntityService<EntertainmentLeisureCard, EntertainmentLeisureDetail>(unitOfWork.EntertainmentLeisure)
    {
    }
}
