using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Repositories.Interfaces.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.MunicipalityEntities
{
    public class PublicEventService(IPublicEventRepository eventRepository) : MunicipalityEntityService<PublicEventCard, PublicEventMobileDetail>(eventRepository)
    {
    }
}
