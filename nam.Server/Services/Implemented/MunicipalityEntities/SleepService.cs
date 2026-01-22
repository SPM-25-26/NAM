using Domain.Entities.MunicipalityEntities;
using Infrastructure.UnitOfWork;

namespace nam.Server.Services.Implemented.MunicipalityEntities
{
    public class SleepService(IUnitOfWork unitOfWork) : MunicipalityEntityService<SleepCard, SleepCardDetail>(unitOfWork.Sleep)
    {
    }
}
