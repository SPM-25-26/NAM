using Domain.Entities.MunicipalityEntities;
using Infrastructure.UnitOfWork;

namespace nam.Server.Services.Implemented.MunicipalityEntities
{
    public class ArtCultureService(IUnitOfWork unitOfWork) : MunicipalityEntityService<ArtCultureNatureCard, ArtCultureNatureDetail>(unitOfWork.ArtCulture)
    {
    }
}
