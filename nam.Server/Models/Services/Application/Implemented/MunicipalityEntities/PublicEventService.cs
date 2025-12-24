using Domain.Entities.MunicipalityEntities;
using Infrastructure.UnitOfWork;

namespace nam.Server.Models.Services.Application.Implemented.MunicipalityEntities
{
    public class PublicEventService(IUnitOfWork unitOfWork) : MunicipalityEntityService<PublicEventCard, PublicEventMobileDetail>(unitOfWork.PublicEvent)
    {
    }
}
