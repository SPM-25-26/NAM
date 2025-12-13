using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Application.Implemented.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Repositories.Interfaces.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.MunicipalityEntities
{
    public class EntertainmentLeisureService(IEntertainmentLeisureRepository entertainmentLeisureRepository) : MunicipalityEntityService<EntertainmentLeisureCard, EntertainmentLeisureDetail>(entertainmentLeisureRepository)
    {
    }
}
