using Domain.Entities.MunicipalityEntities;
using Infrastructure.UnitOfWork;

namespace nam.Server.Services.Implemented.MunicipalityEntities
{
    public class PublicEventService(IUnitOfWork unitOfWork) : MunicipalityEntityService<PublicEventCard, PublicEventMobileDetail>(unitOfWork.PublicEvent)
    {
    }
}
