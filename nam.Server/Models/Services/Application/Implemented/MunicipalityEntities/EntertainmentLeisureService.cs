using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Application.Implemented.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Interfaces;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.MunicipalityEntities
{
    public class EntertainmentLeisureService(IUnitOfWork unitOfWork) : MunicipalityEntityService<EntertainmentLeisureCard, EntertainmentLeisureDetail>(unitOfWork.EntertainmentLeisure)
    {
    }
}
