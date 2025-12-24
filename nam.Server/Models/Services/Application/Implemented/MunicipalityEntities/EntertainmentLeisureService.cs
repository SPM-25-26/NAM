using Domain.Entities.MunicipalityEntities;
using Infrastructure.UnitOfWork;
using nam.Server.Models.Services.Application.Implemented.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.MunicipalityEntities
{
    public class EntertainmentLeisureService(IUnitOfWork unitOfWork) : MunicipalityEntityService<EntertainmentLeisureCard, EntertainmentLeisureDetail>(unitOfWork.EntertainmentLeisure)
    {
    }
}
