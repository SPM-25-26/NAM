using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Interfaces;

namespace nam.Server.Models.Services.Application.Implemented.MunicipalityEntities
{
    public class NatureService(IUnitOfWork unitOfWork) : MunicipalityEntityService<Nature, ArtCultureNatureDetail>(unitOfWork.Nature)
    {
    }
}
