using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Repositories.Interfaces.MunicipalityEntities;

namespace nam.Server.Models.Services.Application.Implemented.MunicipalityEntities
{
    public class NatureService(INatureRepository natureRepository) : MunicipalityEntityService<Nature, ArtCultureNatureDetail>(natureRepository)
    {
    }
}
